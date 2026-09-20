<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_NursingProfessionalCertification, App_Web_glma3lal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
    <script type="text/javascript">
        function numericOnly(obj) {
            obj.value = obj.value.replace(/[^0-9]/g, '');
        }
        function alphabetsOnly(obj) {
            obj.value = obj.value.replace(/[^a-zA-Z]/g, '');
        }
    </script>

<asp:Panel ID="pnlNursingProfessionalCertification" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
    <div class="divGrid">
        <asp:GridView ID="grdNursingProfessionalCertification" Width="98%" runat="server" AllowSorting="true" CssClass="gridview" EmptyDataText="No Nursing Professional Certification found"
            OnRowCommand="grd_RowCommand" AutoGenerateColumns="false" HorizontalAlign="Left">
            <Columns>

                <asp:BoundField DataField="Certification_ID" HeaderText="Certification ID/Name" SortExpression="Certification_ID" />
                <asp:BoundField DataField="Received_From" HeaderText="Received From" SortExpression="Received_From" />
                <asp:BoundField DataField="EXPIRATION" HeaderText="Expiration Date" SortExpression="EXPIRATION" DataFormatString="{0:d}" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditNursingProfessionalsRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                     <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteNursingProfessionalsRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete" 
                                            Visible="<%# CanUserViewDelete()  %>"/>
                 </ItemTemplate>
               </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>

    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddNursingProfessionalCertification" runat="server" ImageUrl="~/Images/add.png" CommandName="NursingProfessionalCertification" OnCommand="lbtnAdd_Click" ToolTip="Add" />
    </div>
    <br />
</asp:Panel>
<div id="nursingDetail" runat="server" visible="false">
    <div style="width:750px;">
        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
        <asp:ValidationSummary ID="vsNursingProfessionalCertifications" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valNursingProfessionalCertifications" />
                    </div>  
        <div class="wdAuto" >

        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel wd200">Certification ID or Name*</span></div>
            <div class="col-sm-9 text-left"><asp:TextBox ID="txtCertificationName" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtCertificationName" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valNursingProfessionalCertifications" ErrorMessage="* Certification Name is required."></asp:RequiredFieldValidator>
            </div>

        </div>
        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel wd200">Received From*</span></div>
            <div class="col-sm-9 text-left"><asp:TextBox ID="txtReceivedFrom" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReceivedFrom" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valNursingProfessionalCertifications" ErrorMessage="* Received From is required."></asp:RequiredFieldValidator>
            </div>

        </div>

    <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel wd200">Expiration*</span></div>
            <div class="col-sm-9 text-left"><asp:TextBox ID="txtExpirationDate" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="calStart" TargetControlID="txtExpirationDate" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtExpirationDate" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valNursingProfessionalCertifications" ErrorMessage="* Expiration is required."></asp:RequiredFieldValidator>
                    <asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="valNursingProfessionalCertifications"   
            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtExpirationDate"  
            ErrorMessage="Select a valid Date of Expiration" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
            SetFocusOnError="true" /> </div>
        </div>  
    </div>
</div>






        

        

