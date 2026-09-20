<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_WorkHistory" Codebehind="WorkHistory.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="pnlUpdate_1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <telerik:RadGrid id="grdWorkHistory" width="100%" runat="server" EnableAriaSupport="true" PageSize="10" AlwaysVisible="true" VirtualItemCount="0"
            AllowPaging="True" AllowSorting="true" AllowFilteringByColumn="true" FilterType="Classic" Skin="PDMSModern" OnSortCommand="grdWorkHistory_SortCommand" OnNeedDataSource="grdWorkHistory_NeedDataSource">
            <MasterTableView autogeneratecolumns="False" datakeynames="">
                <Columns>
                    <telerik:GridBoundColumn DataField="OPERATION" HeaderText="Operation" Visible="true" />
                    <telerik:GridBoundColumn DataField="IS_CURRENT_EMPLOYER" HeaderText="Current Employer" Visible="true" />
                    <telerik:GridBoundColumn DataField="PRACTICE_NAME" HeaderText="Practice/Employer Name" Visible="true" />
                    <telerik:GridBoundColumn DataField="WORKED_FROM" HeaderText="Start Date" Visible="true" DataFormatString="{0:MM/dd/yyyy}" />
                    <telerik:GridBoundColumn DataField="WORKED_TO" HeaderText="End Date" Visible="true" DataFormatString="{0:MM/dd/yyyy}" />
                    <telerik:GridBoundColumn DataField="ADDRESS1" HeaderText="Address 1" Visible="true" />
                    <telerik:GridBoundColumn DataField="ADDRESS2" HeaderText="Address 2" Visible="true" />
                    <telerik:GridBoundColumn DataField="CITY" HeaderText="City" Visible="true" />
                    <telerik:GridBoundColumn DataField="STATE" HeaderText="State" Visible="true" />
                    <telerik:GridBoundColumn DataField="ZIP" HeaderText="ZIP" Visible="true" />
                    <telerik:GridBoundColumn DataField="CONTACT_PHONE" HeaderText="Phone" Visible="true" />
                    <telerik:GridBoundColumn DataField="CONTACT_PHONE_EXT" HeaderText="Phone Ext" Visible="true" />
                    <telerik:GridBoundColumn DataField="FAX1" HeaderText="Fax 1" Visible="true" />
                    <telerik:GridBoundColumn DataField="FAX2" HeaderText="Fax 2" Visible="true" />
                    <telerik:GridBoundColumn DataField="CONTACT_NAME" HeaderText="Contact" Visible="true" />
                    <telerik:GridBoundColumn DataField="CONTACT_EMAIL_ADDRESS" HeaderText="Email 1" Visible="true" />
                    <telerik:GridBoundColumn DataField="CONTACT_EMAIL_ADDRESS_2" HeaderText="Email 2" Visible="true" />
                    <telerik:GridBoundColumn DataField="ADDITIONAL_INFO" HeaderText="Additional Information" Visible="true" />
                    <telerik:GridBoundColumn DataField="REASON_FOR_DEPARTURE" HeaderText="Reason for Departure" Visible="true" />
                    <telerik:GridBoundColumn DataField="CONTACT_EMAIL_ADDRESS_2" HeaderText="Email 2" Visible="true" />
                    <telerik:GridBoundColumn DataField="MILITARY_RESERVE" HeaderText="Active Duty or Military Reserve" Visible="true" />
                    <telerik:GridBoundColumn DataField="username" HeaderText="User Name" Visible="true" />
                    <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="Update Date" Visible="true" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
