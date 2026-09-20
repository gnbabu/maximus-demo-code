<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CategoryOfService" Codebehind="CategoryOfService.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Panel ID="pnlCategoryOfService" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
    <div class="divGrid">
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadGrid ID="rgCategoryOfService" runat="server" Width="100%"
            OnUpdateCommand="rgCategoryOfService_UpdateCommand"
            OnNeedDataSource="rgCategoryOfService_NeedDataSource" OnItemCommand="rgCategoryOfService_ItemCommand" Skin="PDMSModern"
            EnableEmbeddedSkins="false">

            <HeaderStyle CssClass="gridViewHeader" />
            <PagerStyle Mode="NumericPages" CssClass="gridViewPager" />
            <ItemStyle CssClass="gridViewRow" />
            <AlternatingItemStyle CssClass="gridViewAltRow" />
            <SelectedItemStyle CssClass="gridViewSelected" />
            <MasterTableView AllowSorting="true" PageSize="15" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_CATEGORY_OF_SERVICE_INFO_ID,REG_ID" CommandItemDisplay="None" EditMode="PopUp">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="MAX_CATEGORY_OF_SERVICE_TYPE" HeaderText="COS" UniqueName="MAX_CATEGORY_OF_SERVICE_TYPE_ID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CATEGORY_OF_SERVICE_TYPE_NAME" HeaderText="Category Of Service Name" UniqueName="CATEGORY_OF_SERVICE_TYPE_NAME_ID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date" UniqueName="StartDate" DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="End Date">
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" CssClass="formLabelGrid" Text='<%#Eval("EndDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("EndDate", "{0:MM/dd/yyyy}") %>' />
                            <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEndDate" runat="server" />

                            <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="valCategoryOfService"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                ErrorMessage="Select a valid Date of Expiration" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="ImageButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" EditImageUrl="~/Images/edit.png" UpdateImageUrl="../App_Themes/Default/Grid/Update.gif" CancelImageUrl="../App_Themes/Default/Grid/Cancel.gif">
                    </telerik:GridEditCommandColumn>
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <ClientEvents OnPopUpShowing="OnClientShow" />
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddCOS" runat="server" ImageUrl="~/Images/add.png" CommandName="CategoryOfService" OnCommand="lbtnAdd_Click" ToolTip="Add" />
    </div>
    <br />
</asp:Panel>

