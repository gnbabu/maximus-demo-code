<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_PAAssignProcedureGrp" Codebehind="PAAssignProcedureGrp.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style type="text/css">
    .RadGrid_PDMSModern .rgFilterBox {
        background-color: #fff !important;
        font-size: medium !important;
        color: #000;
        height: 30px !important;
        width: 70%;
    }

    caption {
        visibility: hidden !important
    }

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
<script type="text/javascript">
    function populateDates(ddlProcId, txtProcToId, txtDateEffectiveId, txtDateEndId, hdnProcToId, hdnDateEffectiveId, hdnDateEndId, dateEffValue, dateEndValue) {
        var ddlProc = document.getElementById(ddlProcId);
        var txtProcTo = document.getElementById(txtProcToId);
        var txtDateEffective = document.getElementById(txtDateEffectiveId);
        var txtDateEnd = document.getElementById(txtDateEndId);
        var hdnProcTo = document.getElementById(hdnProcToId);
        var hdnDateEffective = document.getElementById(hdnDateEffectiveId);
        var hdnDateEnd = document.getElementById(hdnDateEndId);
        if (ddlProc.value !== "") {
            //alert('ddlProc.value: ' + ddlProc.value);
            // Prepopulate the txtDateEffective with the current date or any desired date
            txtProcTo.value = ddlProc.value;
            hdnProcTo.value = ddlProc.value;
            txtDateEffective.value = dateEffValue;
            hdnDateEffective.value = dateEffValue;
            txtDateEnd.value = dateEndValue;
            hdnDateEnd.value = dateEndValue;
            //alert('End Date: ' + dateEndValue);
        }
    }

</script>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">

    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: PA Assign Procedure Grp"></asp:Literal>
    </p>
    <br />
</asp:Panel>

<telerik:radgrid id="rgPAProcedureGrp" aria-label="PAProcedureGrp" enableariasupport="true" mastertableview-caption="PAProcedureGrp" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgPAProcedureGrp_NeedDataSource" allowfilteringbycolumn="True" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgPAProcedureGrp_InsertCommand" onupdatecommand="rgPAProcedureGrp_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False" onitemdatabound="rgPAProcedureGrp_ItemDataBound" enableembeddedskins="false" onitemcreated="rgPAProcedureGrp_ItemCreated">
    <groupingsettings casesensitive="false" />
    <clientsettings>
        <resizing allowcolumnresize="true" />
        <clientevents onfiltermenushowing="filterMenuShowing" />
    </clientsettings>
    <filtermenu onclientshowing="MenuShowing" cssclass="gridviewFilter" />
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="ID">
        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="CDE_PA_ASSIGN" headertext="CDE PA Assign" uniquename="CDEPAAssign">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="DSC_50" headertext="Desc" uniquename="Desc50">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PROC_FROM" headertext="Proc From" uniquename="ProcFrom" readonly="true">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PROC_TO" headertext="Proc To" uniquename="ProcTo">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PROC_FROM_ORDER" headertext="Proc From Order" uniquename="ProcFromOrder">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PROC_TO_ORDER" headertext="Proc To Order" uniquename="ProcToOrder">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="DTE_EFFECTIVE" headertext="Date Effective" uniquename="DateEffective">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="DTE_END" headertext="Date End" uniquename="DateEnd">
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
                                <span class="ohio-field">PA Assignment Type</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlPAAssignmentType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="ddlPAAssignmentType"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Proc From</span> </div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlProcFrom" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                <%--<asp:TextBox MaxLength="10" aria-label="ProcFrom" ID="txtProcFrom" runat="server" Text='<%# Bind("PROC_FROM") %>' CssClass="formField"></asp:TextBox>--%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="ddlProcFrom"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Proc To</span></div>
                            <div class="col-sm-9 text-left">
                                <%--<asp:DropDownList ID="ddlProcTo" runat="server" CssClass="formDropDown"></asp:DropDownList>--%>
                                <asp:TextBox MaxLength="10" aria-label="ProcTo" ID="txtProcTo" runat="server" ReadOnly="true" Text='<%# Bind("PROC_TO") %>' CssClass="formField"></asp:TextBox>
                                <asp:HiddenField ID="hdnProcTo" runat="server" Value='<%# Bind("PROC_TO") %>' />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtProcTo"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Date Effective</span> </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="10" aria-label="DateEffective" ID="txtDateEffective" runat="server" ReadOnly="true" Text='<%# Bind("DTE_EFFECTIVE") %>' CssClass="formField"></asp:TextBox>
                                <asp:HiddenField ID="hdnDateEffective" runat="server" Value='<%# Bind("DTE_EFFECTIVE") %>' />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtDateEffective"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Date End</span> </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="10" aria-label="DateEnd" ID="txtDateEnd" runat="server" ReadOnly="true" Text='<%# Bind("DTE_END") %>' CssClass="formField"></asp:TextBox>
                                <asp:HiddenField ID="hdnDateEnd" runat="server" Value='<%# Bind("DTE_END") %>' />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtDateEnd"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="row text-center" style="padding-top: 20px;">
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
