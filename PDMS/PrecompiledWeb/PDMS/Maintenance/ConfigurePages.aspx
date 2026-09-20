<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Maintenance_ConfigurePages, App_Web_25ar0nw3" validaterequest="false" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>
<%@ Register Src="~/PopupControls/UploadDocumentControl.ascx" TagName="UploadDocumentControl" TagPrefix="uc" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    Configure Page Content
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
        

    </script>
    <style type="text/css">
#mainForm
{
    margin-left: 15%;  

}
.RightBox
{
    width: 100%;
}
.UserHeader
{
    
    width: 1120px;
    
}
</style> 
                <script type="text/javascript">
    function disableButton(sender, group) {
        Page_ClientValidate(group);
        if (Page_IsValid) {
            sender.disabled = "disabled";
            __doPostBack(sender.name, '');
        }
    }
</script>
<asp:Panel ID="pnlSearch" runat="server" >
    <br /><uc2:Separator ID="UMS01" runat="server" Header="Configure Page Content" Mode="1"/><br />
<div>
    
</div>
</asp:Panel>
<br />
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function RowDblClick(sender, eventArgs) {
                    sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
                }
            </script>
        </telerik:RadCodeBlock>
                
 <telerik:RadGrid RenderMode="Auto" ID="rgConfigurePages" runat="server" Width="760px" AllowPaging="True" AllowSorting="True"
           OnNeedDataSource="rgConfigurePages_NeedDataSource" AutoGenerateColumns="false" AutoGenerateEditColumn="false" AllowFilteringByColumn="false" OnInsertCommand="rgConfigurePages_InsertCommand" OnUpdateCommand="rgConfigurePages_UpdateCommand" OnItemCreated="rgConfigurePages_ItemCreated" OnItemDataBound="rgConfigurePages_ItemDataBound" OnItemCommand="rgConfigurePages_ItemCommand">
            
            <SelectedItemStyle CssClass="gridViewSelected" />
            <EditItemStyle CssClass="gridViewSelected" />
            <ValidationSettings EnableValidation="true" ValidationGroup="ConfigurePagesValidation" CommandsToValidate="rgConfigurePages_ItemCommand" />
            <MasterTableView CommandItemDisplay="top"  
                DataKeyNames="PAGE_CONFIGURATION_ID,document_id,file_name" CssClass="gridViewSmallFont">
                <CommandItemSettings ShowRefreshButton="false" AddNewRecordImageUrl="~/Images/add.png" AddNewRecordText="Add Config" />
                <Columns>
                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="Delete" ImageUrl="~/Images/cancel.png" ButtonType="ImageButton">
                    </telerik:GridButtonColumn> 
                    <telerik:GridBoundColumn DataField="PAGE_NAME" HeaderText="Page Name" UniqueName="PAGE_NAME">
                        
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SECTION_NAME" HeaderText="Section Name" UniqueName="SECTION_NAME">
                        
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LINK_TEXT" HeaderText="Link Text" UniqueName="LINK_TEXT" >
                        
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="REFERENCE_PATH" HeaderText="Reference Path" UniqueName="REFERENCE_PATH">
                        
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SHOW_AS_LINK_OR_TEXT" HeaderText="Show As Link Or Text" UniqueName="SHOW_AS_LINK_OR_TEXT">
                        
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="section_display_order" HeaderText="Display Order" UniqueName="section_display_order">
                        
                    </telerik:GridBoundColumn>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="ImageButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" EditImageUrl="~/Images/edit.png" UpdateImageUrl="../App_Themes/Default/Grid/Update.gif" CancelImageUrl="../App_Themes/Default/Grid/Cancel.gif">
                        
                    </telerik:GridEditCommandColumn>
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" />
                <PagerStyle Mode="NumericPages" ShowPagerText="false" HorizontalAlign="Right" CssClass="radGridViewPager" />
                <AlternatingItemStyle CssClass="gridViewAltRow" />
                <ItemStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
                <CommandItemStyle CssClass="gridViewCommand" />
                <EditFormSettings EditFormType="Template">
                    <FormTemplate>
                        <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="0" rules="none" style="border-collapse: collapse;">
                            <tr class="EditFormHeader">
                                <td colspan="4">
                                    <b>Page Configuration</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPageNameDisplay" runat="server" Text="Page Name" CssClass="formLabel200" /></td>
                                <td>
                                    <asp:TextBox ID="txtPageName" runat="server" Text='<%# Bind("Page_Name") %>'></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblSectionName" runat="server" Text="Section Name" CssClass="formLabel200" /></td>
                                <td>
                                    <asp:TextBox ID="txtSectionName" runat="server" CssClass="formField" Text='<%# Bind("Section_name") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLINK_TEXT" runat="server" Text="Text" CssClass="formLabel200" /></td>
                                <td>
                                    <asp:TextBox ID="txtLINK_TEXT" runat="server" CssClass="formField formField"  TextMode="MultiLine" Text='<%# Bind("link_text") %>'/>
                                    
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Referance Path" CssClass="formLabel200" /></td>
                                <td>
                                    <asp:TextBox ID="txtREFERENCE_PATH" runat="server" CssClass="formField formField" Text='<%# Bind("reference_path") %>' />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSHOW_AS_LINK_OR_TEXT" runat="server" Text="SHOW_AS_LINK_OR_TEXT" CssClass="formLabel200" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlShowAsLinkOrText" runat="server" CssClass="formField" SelectedValue='<%# Bind("show_as_link_or_text") %>' >
                                        <asp:ListItem Text ="Select" Value=""></asp:ListItem>
                                        <asp:ListItem Text ="Link" Value="Link"></asp:ListItem>
                                        <asp:ListItem Text ="Text" Value="Text"></asp:ListItem>
                                        <asp:ListItem Text ="Reference Link" Value="Reference Link"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Display Order" CssClass="formLabel200" />
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="formField" Text='<%# Bind("section_display_order") %>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="cusNumericonly" runat="server" ControlToValidate="txtDisplayOrder"
                                                ValidationExpression="^\d+$" ErrorMessage="* Enter a 3 digit number."  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup = "ConfigurePagesValidation"  Display="Dynamic" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc:UploadDocumentControl ID="ucUploadDocumentControl" runat="server"  />
                                </td>
                            </tr>
                            <tr>

                                <td align="right" colspan="4">
                                    <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox" OnClientClick="disableButton(this,'valBackground')"></asp:Button>&nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" CssClass="buttonBox"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </FormTemplate>
                </EditFormSettings>

            </MasterTableView>
            <ClientSettings>
                <ClientEvents OnRowDblClick="RowDblClick"></ClientEvents>
            </ClientSettings>
        </telerik:RadGrid>  
  
 </div> 
</asp:Content>