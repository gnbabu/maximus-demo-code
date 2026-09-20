<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CMCSpecialties, App_Web_qlfnt5yf" enableviewstate="true" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register src="~/PopupControls/MessageModal.ascx" tagname="MessageBox" tagprefix="mb" %>
<%@ register src="~/PopupControls/SpecialtiesHistory.ascx" tagprefix="uc" tagname="SpecialtiesHistory" %>
<%@ register src="~/PopupControls/RegistrationNavigation.ascx" tagprefix="uc1" tagname="RegistrationNavigation" %>


<div>
    <asp:validationsummary id="vsSpecialties" runat="server" displaymode="List" validationgroup="valSpecialties" />
</div>
<div id="pnlSpecialties" runat="server">
    <div style="width: 100%;">
        <div class="row">
            <div class="col-sm-9">
                <div class="pg-hint4">
                    <span>Primary Specialties are not editable by provider after application submission.</span>
                </div>
            </div>
        </div>
    </div>

    <div class="divGrid">
        <asp:gridview runat="server" width="98%" id="grdSpecialties"
            allowpaging="false" pagesize="1" autogeneratecolumns="False" horizontalalign="Left" cssclass="gridview"
            emptydatatext="No records found">
            <Columns>
                <asp:TemplateField HeaderText="Specialty" ItemStyle-Width="350">
                    <ItemTemplate>
                        <asp:Label ID="lblCmcSpeciality" runat="server" Text='<%# Eval("MMIS_SPECIALTY_TYPE_ID") +" "+ Eval("SPECIALTY_TYPE_NAME") %>  '></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
              <asp:TemplateField HeaderText="Primary">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# (Convert.ToBoolean(Eval("PRIMARY_FLAG")) == true) ? "Yes" : "No" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:gridview>

    </div>
    <asp:label id="lblError" runat="server" text="The specialty has already been added to this registration. Please select a different one" forecolor="Red" visible="false" />

</div>

<div style="height: 200px"></div>

<div id="divCMCEnrollmentCount" runat="server" class="pg-hint4">
    <asp:label id="lblCMCEnrollmentCount" runat="server" text="CMC Qualifying Enrollment Count :" />
    <asp:label id="lblDisplayCMCEnrollmentCount" runat="server"/>
</div>

