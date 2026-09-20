<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Maintenance_User, App_Web_25ar0nw3" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/TransferOwnership.ascx" TagName="TransferProvider" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    User Maintenance
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
            <div class="WhiteBox" >
    <style type="text/css">
        .fieldTable>tbody>tr>td:nth-child(1)
        {
            width:50%;
        }
        .formLabel
        {
            font-weight:bold;
        }
    </style>
    <script type="text/javascript">
        function keyUP(txt) {
            if (document.getElementById("ctl00_MainContent_UMS02").value.length
                + document.getElementById("ctl00_MainContent_UMS08").value.length
                + document.getElementById("ctl00_MainContent_txtSearchOrgName").value.length 
                > 0) {
                //document.getElementById("ctl00_MainContent_UMS04").disabled = false;
            }
            else {
                //document.getElementById("ctl00_MainContent_UMS04").disabled = true;
            }
        }

    </script>
    <style type="text/css">
#mainForm
{
    margin-left: 15%;  

}
.RightBox
{
    width: 860px;
}
.UserHeader
{
    
    width: 1120px;
    
}
</style> 

<asp:Panel ID="pnlSearch" runat="server" DefaultButton="UMS04">
   <div class="boxContainer"><asp:Label ID="lblHeader2" Text="User Maintenance Search" runat="server" CssClass="boxLabel" /></div>
<br />
<div>
    <div id="useradminsearchdtls">
        <div class="row">
            <div class="col-sm-4 text-right"><asp:Label ID="lblUserId" runat="server" Text="User ID" /></div>
             <asp:TextBox runat="server" ID="dummyUserId" aria-Label="DummyUserId" style="display:none;"></asp:TextBox> <!-- Dummy field to solve browser autofill issue -->

            <div class="col-sm-8 text-left"><asp:TextBox ID="UMS02" runat="server" aria-Label="UserId" CssClass="formField"  onkeyup="keyUP(this)" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><asp:Label ID="lblContactName" runat="server" Text="Contact Name" /></div>
            <div class="col-sm-8 text-left"><asp:TextBox ID="UMS08" runat="server" aria-Label="Contact Name" CssClass="formField" onkeyup="keyUP(this)" autocomplete="off"/></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><asp:Label ID="lblOrgName" runat="server" Text="Organization Name" /></div>
            <div class="col-sm-8 text-left"><asp:TextBox ID="txtSearchOrgName" runat="server" CssClass="formField" onkeyup="keyUP(this)" autocomplete="off" aria-Label="Organization Name"/></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><asp:Label ID="lblTaxId" runat="server" Text="Tax ID" /></div>
             <asp:TextBox runat="server" aria-Label="DummyTaxId" ID="TextBox1" style="display:none;"></asp:TextBox> <!-- Dummy field to solve browser autofill issue -->
            <div class="col-sm-8"><asp:TextBox ID="txtSearchTaxId" runat="server" CssClass="formField"  aria-Label="Tax ID" onkeyup="keyUP(this)" autocomplete="off" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><asp:Label ID="lblNPI" runat="server" Text="NPI" /></div>
            <asp:TextBox runat="server" ID="TextBox2" style="display:none" aria-label="DummyNPI" ></asp:TextBox> <!-- Dummy field to solve browser autofill issue -->
            <div class="col-sm-8"><asp:TextBox ID="txtSearchNPI" runat="server" CssClass="formField" aria-label="NPI" onkeyup="keyUP(this)" autocomplete="off" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><asp:Label ID="lblRole" runat="server" Text="Role" /></div>
            <div class="col-sm-8 text-left"><asp:DropDownList ID="cboxSearchRole" AutoPostBack="false" AppendDataBoundItems="True" aria-label="Search role" runat="server" CssClass="formDropDown" autocomplete="off" ></asp:DropDownList></div>
        </div>
        <%--<div class="row"><td colspan="2">&nbsp;</td></div>--%>
        <div class="row">
            <div class="col-sm-12 text-center">
              <%--  <div class="btnBox btnBoxCenter">--%>
                    <asp:Button ID="UMS04" runat="server" Text="Search" onclick="UMS04_Click" CssClass="buttonBox" />
                    <asp:Button ID="btnIOPUser" runat="server" Text="Add IOP User" onclick="btnIOPUser_Click" CssClass="buttonBox" />
                    <asp:Button ID="btnAddNew" runat="server" Text="Add User" Visible="false" onclick="btnAddNew_Click" CssClass="buttonBox" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" CssClass="buttonBox" />
                <%--</div>--%>
            </div>
           <%--<td>             <asp:Button ID="btnTransferReg" runat="server" Text="Transfer Ownership" onclick="btnTransferReg_Click" CssClass="buttonBox"  /></td> --%>
        </div>
    </div>
    <%--<uc:FormField ID="UMS02" runat="server" LabelText="User Name / User ID" FieldType="TextBox" />--%>
    <%--<uc:FormField ID="UMS08" runat="server" LabelText="Contact Name" FieldType="TextBox" />--%>
</div>
     <asp:TextBox runat="server" ID="TextBox3" style="display:block; height:1px; width:1px" aria-Label="Dummytext"></asp:TextBox> <!-- Dummy field to solve browser autofill issue -->
</asp:Panel>
<br />
   
<div style="text-align:center; width:100%">
        <asp:Panel id="pnlSearchResults" runat="server">
            <mms:SortablePagingGridView
    ID="grdFilteredUsers"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnRowCommand="grdFilteredUsers_RowCommand"
    OnRowDataBound="grdFilteredUsers_RowDataBound"
    OnPageIndexChanging="grdFilteredUsers_PageIndexChanging"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    DataKeyNames="USER_NAME,reg_id,Tax_Id,Medicaid_id">
       
            <%--<asp:GridView runat="server" Width="100%" ID="grdFilteredUsers" DataKeyNames="USER_NAME,reg_id,Tax_Id,Medicaid_id" AutoGenerateColumns="false" 
                HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grdFilteredUsers_RowCommand" 
                OnRowDataBound="grdFilteredUsers_RowDataBound" CellPadding="5" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="grdFilteredUsers_PageIndexChanging">--%>
                <Columns>
                    <asp:TemplateField HeaderText="Contact Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnContact" runat="server" Text='<%# Bind("CONTACT_NAME") %>' DataTextField="CONTACT_NAME" CommandName="user" HeaderText="Contact Name" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User ID">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnUser" runat="server" Text='<%# Bind("USER_NAME") %>' DataTextField="USER_NAME" CommandName="user" HeaderText="User ID" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Org. Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnGroup" runat="server" Text='<%# Bind("GROUP_NAME") %>' DataTextField="GROUP_NAME" CommandName="user" HeaderText="Organization Name" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Medicaid ID">
                        <ItemTemplate>
                            <asp:Label ID="lbtnMedicaidID" runat="server" Text='<%# Bind("Medicaid_id") %>' DataTextField="Medicaid_id"  HeaderText="Medicaid ID"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NPI">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnNPI" runat="server" Text='<%# Bind("NPI") %>' DataTextField="NPI" CommandName="user" HeaderText="NPI" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tax Id">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnTaxId" runat="server" Text='<%# Bind("Tax_Id") %>' DataTextField="Tax_Id" CommandName="user" HeaderText="Tax Id" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Provider Type">
                        <ItemTemplate>
                            <asp:Label ID="lblProviderTypeName" runat="server" Text='<%# Bind("PROVIDER_TYPE_NAME") %>' DataTextField="PROVIDER_TYPE_NAME"  HeaderText="PROVIDER TYPE NAME"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("PracticeLocationZip") %>' DataTextField="PracticeLocationZip"  HeaderText="Location"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Taxonomy Code">
                        <ItemTemplate>
                            <asp:Label ID="lblTaxonomyName" runat="server" Text='<%# Bind("TAXONOMY_NAME") %>' DataTextField="TAXONOMY_NAME"  HeaderText="Taxonomy Code"  />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Active">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtActive" runat="server" Text='<%# Bind("Active") %>' DataTextField="Active" CommandName="user" HeaderText="Active" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Locked">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtLocked" runat="server" Text='<%# Bind("IsLockedOut") %>' DataTextField="IsLockedOut" CommandName="user" HeaderText="Locked" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IOP User?">
                        <ItemTemplate>
                            <asp:Label ID="lblOHID" runat="server" Text='<%# Bind("IsOHID") %>' DataTextField="IsOHID"  HeaderText="IOP User?"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Transfer Ownership">
                        <ItemTemplate>
                            
                            <asp:LinkButton ID="chkTransfer" runat="server"  Text="Transfer Ownership" CommandName="transfer" HeaderText="Transfer Ownership" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'  />
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>

                <%--<PagerStyle cssClass="gridpager" HorizontalAlign="Left" />  
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" /> 
                <FooterStyle CssClass="gridViewFooter" />--%>
         <%--   </asp:GridView>--%>
                     </mms:SortablePagingGridView>
            <asp:DataList ID="dlPager" CellPadding="5" RepeatDirection="Horizontal" runat="server" OnItemCommand="dlPager_ItemCommand" RepeatColumns="20" Visible="false">
                <ItemStyle Wrap="true" />
                <ItemTemplate>
                    <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
                </ItemTemplate>
            </asp:DataList>
            <br />
            <br />

                <asp:HiddenField ID="hidIndex" runat="server" />
            </asp:Panel>

    </div>
    <!-- Dialog for entry of User -->
    <asp:Panel ID="pnlUser" runat="server" CssClass="popControl" Style="display: none" Width="500px">
    <asp:Panel ID="Panel2" CssClass="popControlHeader" runat="server" HorizontalAlign="Left">
        <div style="text-align :left">
            &nbsp;&nbsp;<asp:Label ID="Label2" CssClass="bodyTextBold" runat="server" Text="Enter User"
                ForeColor="White"></asp:Label></div>
    </asp:Panel>
    <div style="text-align :center">
        <br />
        <table>
            <tr>
                <td class="formLabel"><asp:Label ID="lblUsername" runat="server" Text="Username:" /></td>
                <td class="formEntry"><asp:TextBox ID="txtUserName" aria-Label="Username" runat="server" CssClass="formField" />
                </td>
            </tr>
            <asp:Panel ID="pnlCreate" runat="server">
                <tr>
                    <td class="formLabel"><asp:Label ID="lblPassword" runat="server" Text="Password:" /></td>
                    <td class="formEntry">
                        <asp:TextBox ID="txtPassword" aria-Label="Password"  runat="server" CssClass="formField" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td class="formLabel"><asp:Label ID="lblConfirmPwd" runat="server" Text="Confirm Password:" /></td>
                    <td class="formEntry">
                    <asp:TextBox ID="txtConfirmPassword"  aria-Label="Confirm Password" runat="server" CssClass="formField" TextMode="Password" />
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td class="formLabel"><asp:Label ID="lblEmail" runat="server" Text="Email:" /></td>
                <td class="formEntry"><asp:TextBox ID="txtEmail"  aria-Label="Email" runat="server" CssClass="formField220" />
                </td>
            </tr>
            <tr>
                <td class="formLabel"><asp:Label ID="lblApproved" runat="server" Text="Is Approved:" /></td>
                <td class="formEntry">
                    <asp:RadioButtonList ID="rblIsApproved" BorderStyle="None" CellPadding="0" CellSpacing="0" 
                        RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="RadioList">  
                        <asp:ListItem Value="true">True</asp:ListItem>  
                        <asp:ListItem Value="false">False</asp:ListItem>  
                    </asp:RadioButtonList>
                 </td>
            </tr>
            <tr>
                <td class="formLabel"><asp:Label ID="lbRole" runat="server" Text="Role:" /></td>
                <td class="formEntry">
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="DropDownList" />
                </td>
            </tr>
        </table>
        <br />
            <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" CssClass="buttonBox" />&nbsp;
            <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="buttonBox" />
        <br />
        <br />
    </div>
    </asp:Panel>
    <cc1:ModalPopupExtender ID="mpeUser" runat="server" BackgroundCssClass="modalBackground"
        Drag="False" CancelControlID="btnCancel" PopupControlID="pnlUser" TargetControlID="ButtonDummy">
    </cc1:ModalPopupExtender>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />

    <cc1:ModalPopupExtender ID="mpe1" runat="server" PopupControlID="pnlModal1" TargetControlID="Button1"
   BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlHeader1">
    </cc1:ModalPopupExtender>

    <asp:Panel ID="pnlModal1" runat="server" CssClass="modalPopup" align="center" Style="display: none;min-height: 50%; min-width: 50%; width:auto;">
           <asp:Panel ID="pnlHeader1" CssClass="popHeader" runat="server" >
           <div class="popTitle">          
            <asp:Label ID="lblMpeTitle1"  runat="server" Text="Transfer Ownership"  />
           </div>
           </asp:Panel>
      <asp:Panel ID="pnlMain1" runat="server" Style="margin-right: 10px;width: auto;height: auto;">
                <asp:MultiView ID="mltPopup" runat="server">
                    <asp:View ID ="vwTransferProvider" runat="server">
                        <br />
                        <cc2:TransferProvider ID="ucTransferProvider" runat="server" />
                    </asp:View>

            </asp:MultiView>
             <%-- <div class="btnBox" style="padding-top: 20px; padding-right: 10px;">
                <asp:Button ID="btnTransfer" runat="server" Text="Transfer" CssClass="buttonBoxFocus" OnClick="btnTransfer_Click"
                    CausesValidation="true" />           
                <asp:Button ID="btnCancelmpe1" runat="server" Text="Cancel" CssClass="buttonBox"
                    CausesValidation="false" />
              </div>--%>
    </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" ID="Button1" Style="display: none" Text="ButtonDummy" />
    <uc1:MessageBox ID="MessageBox2" runat="server" />
                </div>
</asp:Content>