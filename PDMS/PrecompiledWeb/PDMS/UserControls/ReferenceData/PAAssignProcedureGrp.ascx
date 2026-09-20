<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_PAAssignProcedureGrp, App_Web_iq0r534d" %>
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
    visibility:hidden !important
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
    
        <h1 style="text-align: center;font-size: 25px;font-weight: bold;"><asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: PA Assign Procedure Grp"></asp:Literal></h1>
   
</asp:Panel>

<telerik:RadGrid ID="rgPAProcedureGrp" aria-label="PAProcedureGrp" enableariasupport="true" MasterTableView-Caption="PAProcedureGrp" runat="server" RenderMode="Lightweight" AllowPaging="True" AllowSorting="True"
    OnNeedDataSource="rgPAProcedureGrp_NeedDataSource" AllowFilteringByColumn="True" Skin="PDMSModern"
    CellSpacing="0" GridLines="None" OnInsertCommand="rgPAProcedureGrp_InsertCommand" OnUpdateCommand="rgPAProcedureGrp_UpdateCommand"
    AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnItemDataBound="rgPAProcedureGrp_ItemDataBound" EnableEmbeddedSkins="false" OnItemCreated="rgPAProcedureGrp_ItemCreated">
    <GroupingSettings CaseSensitive="false" />
    <ClientSettings>
        <Resizing AllowColumnResize="true" />
        <ClientEvents OnFilterMenuShowing="filterMenuShowing" />
    </ClientSettings>
    <FilterMenu OnClientShowing="MenuShowing" CssClass="gridviewFilter" />
    <MasterTableView CommandItemDisplay="Top" GridLines="None"
        DataKeyNames="ID">
        <Columns>
            <telerik:GridEditCommandColumn>
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn DataField="CDE_PA_ASSIGN" HeaderText="CDE PA Assign" UniqueName="CDEPAAssign">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="DSC_50" HeaderText="Desc" UniqueName="Desc50">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PROC_FROM" HeaderText="Proc From" UniqueName="ProcFrom" ReadOnly="true">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PROC_TO" HeaderText="Proc To" UniqueName="ProcTo">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PROC_FROM_ORDER" HeaderText="Proc From Order" UniqueName="ProcFromOrder">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PROC_TO_ORDER" HeaderText="Proc To Order" UniqueName="ProcToOrder">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="DTE_EFFECTIVE" HeaderText="Date Effective" UniqueName="DateEffective">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="DTE_END" HeaderText="Date End" UniqueName="DateEnd">
            </telerik:GridBoundColumn>
        </Columns>
        <EditFormSettings EditFormType="Template">
            <EditColumn UniqueName="EditCol">
            </EditColumn>
            <FormTemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">PA Assignment Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlPAAssignmentType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="ddlPAAssignmentType"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Proc From </div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlProcFrom" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                <%--<asp:TextBox MaxLength="10" aria-label="ProcFrom" ID="txtProcFrom" runat="server" Text='<%# Bind("PROC_FROM") %>' CssClass="formField"></asp:TextBox>--%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="ddlProcFrom"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Proc To</div>
                            <div class="col-sm-9 text-left">
                                <%--<asp:DropDownList ID="ddlProcTo" runat="server" CssClass="formDropDown"></asp:DropDownList>--%>
                                <asp:TextBox MaxLength="10" aria-label="ProcTo" ID="txtProcTo" runat="server" ReadOnly="true" Text='<%# Bind("PROC_TO") %>' CssClass="formField"></asp:TextBox>
                                <asp:HiddenField ID="hdnProcTo" runat="server" Value='<%# Bind("PROC_TO") %>' />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtProcTo"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Date Effective </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="10" aria-label="DateEffective" ID="txtDateEffective" runat="server" ReadOnly="true" Text='<%# Bind("DTE_EFFECTIVE") %>' CssClass="formField"></asp:TextBox>
                                <asp:HiddenField ID="hdnDateEffective" runat="server" Value='<%# Bind("DTE_EFFECTIVE") %>' />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtDateEffective"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Date End </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="10" aria-label="DateEnd" ID="txtDateEnd" runat="server" ReadOnly="true" Text='<%# Bind("DTE_END") %>' CssClass="formField"></asp:TextBox>
                                <asp:HiddenField ID="hdnDateEnd" runat="server" Value='<%# Bind("DTE_END") %>' />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtDateEnd"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="row text-center">
                            <%--<td colspan="2" align="center">--%>
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                CommandName="Cancel"></asp:Button>
                            <%--</td>--%>
                        </div>
                    </div>
                </div>
            </FormTemplate>
            <PopUpSettings ScrollBars="None" />
        </EditFormSettings>
    </MasterTableView>
</telerik:RadGrid>