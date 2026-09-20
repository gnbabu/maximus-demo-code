<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CPCSpecialties"
    EnableViewState="true" Codebehind="CPCSpecialties.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="mb" %>
<%@ Register Src="~/PopupControls/SpecialtiesHistory.ascx" TagPrefix="uc" TagName="SpecialtiesHistory" %>


<div>
    <asp:ValidationSummary ID="vsSpecialties" runat="server" DisplayMode="List" ValidationGroup="valSpecialties" />
</div>
<div style="width: 100%;">
    <div class="row">
        <div class="col-sm-9">
            <div class="pg-hint4">
                <div class="popTitle">
                    <p>
                        <asp:Label ID="lblSpecialtiesTitle" CssClass="bodyTextBold groupMemberTitle" runat="server" Text="Next Program Year " />
                    </p>

                </div>
            </div>
        </div>
    </div>
</div>

<div id="pnlSpecialties" runat="server">
    <p>
        <span class="pageHeader">Next Program Year </span>
    </p>
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdSpecialties"
            AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grd_RowCommand" OnRowCreated="grd_RowDataCreated">
            <Columns>
                <asp:BoundField DataField="SPECIALTY_TYPE_NAME" HeaderText="Primary Specialty" ItemStyle-Width="350" />
              <asp:TemplateField HeaderText="Primary">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# (Convert.ToBoolean(Eval("PRIMARY_FLAG")) == true) ? "Yes" : "No" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="ENROLL_STATUS_DESC" HeaderText="Enroll Status" />    
                <asp:BoundField DataField="ENROLLMENT_STATUS_REASONS_DESC" HeaderText="Enroll Status Reason" />   
                          <%--  <asp:TemplateField HeaderText="Enroll Status Reason">
                                <ItemTemplate>
                                    <asp:Label ID="lblEnrollStatusReason" runat="server" Text='<%# Convert.ToString(Eval("ENROLLMENT_STATUS_REASONS_DESC")) %>' Visible="<%# CanCPCUserViewEnrollCol( ((GridViewRow) Container).RowIndex)  %>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                <asp:TemplateField HeaderText ="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditSpecialtiesRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        ImageUrl="~/Images/edit.png" ToolTip="Edit" AlternateText="Edit"
                                      Visible="<%# CanCPCUserViewEdit( ((GridViewRow) Container).RowIndex)  %>" />
                                  
                                </ItemTemplate>
                            </asp:TemplateField>
                 <asp:TemplateField HeaderText ="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteSpecialtiesRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"
                                      Visible="<%# CanCPCUserViewDelete( ((GridViewRow) Container).RowIndex)  %>" />
                                  
                                </ItemTemplate>
                            </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
       <div id="divSpecAddNew" runat="server">
                        <div class="divHistoryAndAdd" id="divAddButton" runat="server">
                    <asp:ImageButton ID="btnAddSpecialties" runat="server" ImageUrl="~/Images/add.png"  CommandName="Specialties" OnCommand="lbtnAdd_Click" ToolTip="Add"/>
                 

                </div>
         <asp:Label ID="lblIsKidsCriteria" runat="server" ForeColor="Red" Visible="false" />
         <div><div ID="kidsSplAddnew" runat="server" style="color:red">To add the Kids specialty,Click the Add new button </div>
         <asp:Label ID="lblError" runat="server" Text="The specialty has already been added to this registration. Please select a different one" ForeColor="Red" Visible="false" />

         </div></div>
         <div id="divSpecialtyDetail" visible="false" runat="server">
               
                <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="ParentTable" runat="server">
                            <colgroup>
                                <col width="33%" />
                                <col width="53%" align="left" />
                                <col width="20%" align="left" />
                            </colgroup>
                            <tr>
                                <td><span class="formLabel">Specialty*</span></td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSpecialty" runat="server" EnableViewState="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="reqCategory" ValidationGroup="valSpecialties"
                                        ControlToValidate="ddlSpecialty" ErrorMessage="*Select a Specialty" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" InitialValue="" />
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblPDMSSpecialty" runat="server" />
                                </td>
                            </tr>
                          
                            <tr>
                                <td><span class="formLabel">Start Date*</span></td>
                                <td align="left">
                                    <asp:TextBox ID="txtSpecStartDate" runat="server" CssClass="formField formField"></asp:TextBox>
                                   
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label2" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td><span class="formLabel">End Date</span></td>
                                <td align="left">
                                    <asp:TextBox ID="txtSpecEndDate" runat="server" CssClass="formField formField"></asp:TextBox>
                                    
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" />
                                </td>
                            </tr>
                              
                            <tr>
                                 
                                <td>&nbsp;</td>
                                <td align="left">
                                    <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="pnlUpdate">
                                        <ProgressTemplate>
                                            <div style="padding-right: 30px">
                                                <img src="../Images/ajax-loader.gif" />
                                                Loading ...
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:HiddenField ID="hiddenRegSpecialtyID" runat="server" />
            </div>
    </div>
     
</div>
<div style="height: 200px"></div>
<div id="pnlLastYearSpecialties" runat="server">
    <div style="width: 100%;">
        <p>
            <span class="pageHeader">Previous Years Enrollment </span>
        </p>
    </div>
    <div>
        <asp:GridView runat="server" Width="98%" ID="grdLastYearSpecialties"
            AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grd_RowCommand">
            <Columns>
                <asp:BoundField DataField="SPECIALTY_TYPE_NAME" HeaderText="Primary Specialty" ItemStyle-Width="350" />
                <asp:TemplateField HeaderText="Primary">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# (Convert.ToBoolean(Eval("PRIMARY_FLAG")) == true) ? "Yes" : "No" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="ENROLL_STATUS_DESC" HeaderText="Enroll Status" />
                            <asp:TemplateField HeaderText="Enroll Status Reason">
                                <ItemTemplate>
                                    <asp:Label ID="lblEnrollStatusReason" runat="server" Text='<%# Convert.ToString(Eval("ENROLLMENT_STATUS_REASONS_DESC")) %>'></asp:Label>
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

    <br />
</div>
<div>
    <asp:Label ID="lblDuplicate" runat="server" Text="The specialty has already been added to this registration. Please select a different one" ForeColor="Red" Visible="false" />
    <asp:HiddenField ID="hdnRegSpecialtyID" runat="server" />
</div>

<br />
<asp:Button ID="ButtonDummy1" runat="server" Style="display: none" Text="ButtonDummy1" />
<mb:MessageBox ID="MessageBox2" runat="server"  />
<asp:HiddenField ID="hdnEnrollStsID" runat="server" />
<asp:HiddenField ID="hdnMMISSpecType" runat="server" />
