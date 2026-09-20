<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_WaiverServices" Codebehind="WaiverServices.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<style type="text/css"> 
    .separatorBreak {
         height:10px; 
        /*background-color:White; 
        height:5px; 
	    -moz-box-shadow: none;
	    -webkit-box-shadow: none;
	    box-shadow: none;*/    
    } 
    .RadGrid .rgInput, .RadGrid .rgEditRow > td > [type='text'], .RadGrid .rgEditForm td > [type='text'], .RadGrid .rgBatchContainer > [type='text'], .RadGrid .rgFilterBox, .RadGrid .rgFilterApply, .RadGrid .rgFilterCancel {
        width:100%;
    }
    .chkBoxAlign {
        float: left; 
        padding-right:5px;
        padding-top:5px;
    }
</style>
<script type="text/javascript">
    function textBoxValueChanged(rowIndex) {

        var masterTable = $find("<%= rgEPDSpecialties.ClientID %>").get_masterTableView();
            //masterTable.editItem(masterTable.get_dataItems()[rowIndex].get_element());
            var row = masterTable.get_dataItems()[rowIndex];
            //for (var i = 0; i < row.length; i++) {
            //if(row._element.children['1'].children[0].children[0].value row._element.children['5'].children[0].value
            //index 8 if IDDD
            row._element.children['7'].children[0].disabled = false;
            row._element.children['7'].children[1].disabled = false;
            row._element.children['7'].removeAttribute("disabled");
            //}
            //var chkBox = row.findElement("SPECIALTY_SELECTED");
            //chkBox.disabled = true;
    }


    function RowValueChanged(gridID, rowIndex) {
        var masterTable = $find(gridID).get_masterTableView();
         var row = masterTable.get_dataItems()[rowIndex];
          
         var count = row._element.children.length;

         row._element.children[count - 1].children[0].disabled = false;
         row._element.children[count - 1].children[1].disabled = false;
         row._element.children[count - 1].removeAttribute("disabled");
         //}
         //var chkBox = row.findElement("SPECIALTY_SELECTED");
         //chkBox.disabled = true;
     }

        function comboSelectedIndexChanged(sender, args) {
            console.log("combo value changed");
        }
</script>

<asp:ValidationSummary ID="valEPDWaiver" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="EPDWaiverValidation" />
<uc1:Separator ID="Separator5" runat="server" Header="Specialties" Mode="1" />
<div>
    <br />
    <div>Indicate on this page the services which you are applying to provide. You must apply for at least one service and can apply for multiple services. 
    </div>
    <div id="IDDMessage" runat="server"><br />To apply for services that do not have a “Provider Requests” column, click the box in the header to indicate you are applying for that service and then provide a number in the corresponding “Capacity” section as indicated.</div>
    <br />
    <div class="divGrid">
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>

        <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgEPDSpecialties" runat="server" Width="100%" Visible="false" PageSize="50"
            OnNeedDataSource="grid_NeedDataSource"
            OnUpdateCommand="rgEPDSpecialties_UpdateCommand"           
            OnItemCommand="rgEPDSpecialties_ItemCommand"
            OnDataBound="rgEPDSpecialties_DataBound" AllowMultiRowEdit="true" OnPreRender="rgEPDSpecialties_PreRender" OnItemCreated="rgEPDSpecialties_ItemCreated">
             
            <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="rgEPDSpecialties_ItemCommand" />
            <MasterTableView AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, REG_SPECIALTY_APPROVAL_ID" CommandItemDisplay="None" EditMode="InPlace">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridCheckBoxColumn DataField="SPECIALTY_APPROVED" HeaderText="LTC Approved Services" UniqueName="chkSpecialtyApproved" Visible="true"></telerik:GridCheckBoxColumn>
                    <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="" UniqueName="chkSpecialty" Display="true"  ></telerik:GridCheckBoxColumn>
                    <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_NAME" HeaderText="Service Name" UniqueName="SPECIALTY_TYPE_NAME"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="40px"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE">
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                            <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel"></telerik:GridEditCommandColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>

        <asp:Panel ID="pnlArtTherapy" runat="server" Visible="false">
            <div style="width: 98%">
                <asp:CheckBox ID="chkArtTherapySpecialties" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepArtTherapySpecialties" runat="server" Header="Creative Art Therapies (See Section 1918, Chapter 19 of Title 29, DCMR,)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgArtTherapySpecialties" runat="server" Width="100%"
                OnNeedDataSource="grid_NeedDataSource" 
                OnUpdateCommand="gridServiceType_UpdateCommand"
                OnDataBound="gridServiceType_DataBound" AllowMultiRowEdit="true" 
                OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender"> 
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, REG_SPECIALTY_CATEGORY_ID, SPECIALTY_CATEGORY_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace" Height="10px">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_NAME" HeaderText="Service Name" UniqueName="SPECIALTY_TYPE_NAME" ReadOnly="true"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity" ></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn> 
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
    <br />


        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlBehavioralSupport" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkBehavioralSupportSpecialties" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />

                <uc1:Separator ID="sepBehavioralSupportSpecialties" runat="server" Header="Behavioral Supports (See Section 1919, Chapter 19 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgBehavioralSupportSpecialties" runat="server" Width="100%"
                
                OnNeedDataSource="grid_NeedDataSource"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" 
                OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender">
                
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlDayHabilitationSpecialties" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkDayHabilitationSpecialties" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />

                <uc1:Separator ID="sepDayHabilitationSpecialties" runat="server" Header="Day Habilitation Services (See Section 1920, Chapter 9 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgDayHabilitationSpecialties" runat="server" Width="100%"
                
                OnNeedDataSource="grid_NeedDataSource"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
               
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <table id="tblFootage" >
                <tr>
                    <td class="formLabel wdAuto" style="height: 20px; margin-top: 7px;">Enter Facility Footage</td>
                    <td>
                        <ew:NumericBox ID="nbFootage" runat="server" DecimalPlaces="0" PositiveNumber="True" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlEmploymentReadiness" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkEmploymentReadiness" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepEmploymentReadiness" runat="server" Header="Employment Readiness (See Section 1922, Chapter 9 of Title 29, DCMR)" Mode="1" />
            </div>

            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgEmploymentReadiness" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
           
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlEnvAccessibilities" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkEnvAccessibilities" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepEnvAccessibilities" runat="server" Header="Environmental Accessibilities Adaptations (See Section 926, Chapter 19 of Title 29, DMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgEnvAccessibilities" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" 
                OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender">
               
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />
        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlFamilyTraining" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkFamilyTraining" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />

                <uc1:Separator ID="sepFamilyTraining" runat="server" Header="Family Training (See Section 1924, Chapter 19 of Title 29, DMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgFamilyTraining" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" 
                OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender">
               
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />
        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlHostHome" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkHostHome" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepHostHome" runat="server" Header="Host Home without Transportation (See Section 1915, Chapter 19 of Title 29, DMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgHostHome" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" 
                OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender">
                
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                       <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <table id="tblHostHome" >
                <tr>
                    <td class="formLabel wdAuto" style="height: 20px; margin-top: 7px;">Number of Host Homes</td>
                    <td>
                        <ew:NumericBox ID="nbHostHomes" runat="server" DecimalPlaces="0" PositiveNumber="True" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlIndividualizedDay" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkIndividualizedDay" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepIndividualizedDay" runat="server" Header="Individualized Day Services (See Section 1924, Chapter 9 of Title 29, DMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgIndividualizedDay" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" 
                OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender">
                
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlInHomeSupports" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkInHomeSupports" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepInHomeSupports" runat="server" Header="In-Home Supports (See Section 1916, Chapter 19 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgInHomeSupports" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" 
                OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender">
                
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlPERS" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkPERS" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />

                <uc1:Separator ID="sepPERS" runat="server" Header="Personal Emergency Response System (See Section 1927, Chapter 19 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgPERS" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated"
                OnPreRender="grid_PreRender">
             
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlRespite" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkRespite" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepRespite" runat="server" Header="Respite (See Section 1930, Chapter 19 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgRespite" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="Provider Requests" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlResHabilitation" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkResHabilitation" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepResHabilitation" runat="server" Header="Residential Habilitation (See Section 1929, Chapter 9 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgResHabilitation" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlSkilledNursing" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkSkilledNursing" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />

                <uc1:Separator ID="sepSkilledNursing" runat="server" Header="Skilled Nursing (See Section 1931, Chapter 9 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgSkilledNursing" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlSpeechHearing" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkSpeechHearing" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepSpeechHearing" runat="server" Header="Speech, Hearing & Language Therapy (See Section 1932, Chapter 9 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgSpeechHearing" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                       <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px" ></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlSupportEmployment" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkSupportEmployment" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepSupportEmployment" runat="server" Header="Supported Employment Services-Individual and small group services (See Section 1933, Chapter 9 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgSupportEmployment" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlSupportedLiving" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkSupportedLiving" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepSupportedLiving" runat="server" Header="Supported Living Services with transportation (See Section 1934, Chapter 9 of Title 29, DCMR)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgSupportedLiving" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
             
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID,SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
           
            <table class="gridView" style="width:100%" >
                <thead class="rgHeader">
                    <tr>
                        <td></td>
                        <td>Vehicle Type</td>
                        <td>List the number of vehicles for each category</td>
                    </tr>
                </thead>
                <tbody>
                    <tr class="rgRow">
                        <td>
                            <asp:CheckBox ID="chkPersonalVehicles" runat="server" /></td>
                        <td>Direct Support Professional personal motor vehicles</td>
                        <td>
                            <ew:NumericBox ID="nbPersonalVehicles" runat="server" DecimalPlaces="0" PositiveNumber="True" /></td>
                    </tr>
                    <tr class="rgRow">
                        <td colspan="2">Agency Motor Vehicles</td>
                        <td></td>
                    </tr>
                    <tr class="rgRow">
                        <td>
                            <asp:CheckBox ID="chkAgencyCar" runat="server" /></td>
                        <td>Car</td>
                        <td>
                            <ew:NumericBox ID="nbAgencyCar" runat="server" DecimalPlaces="0" PositiveNumber="True" /></td>
                    </tr>
                    <tr class="rgRow">
                        <td>
                            <asp:CheckBox ID="chkAgencyMinivan" runat="server" /></td>
                        <td>Minivan</td>
                        <td>
                            <ew:NumericBox ID="nbAgencyMinivan" runat="server" DecimalPlaces="0" PositiveNumber="True" /></td>
                    </tr>
                    <tr class="rgRow">
                        <td>
                            <asp:CheckBox ID="chkAgencySmallVan" runat="server" /></td>
                        <td>Small Passenger Van (up to 15 passengers)</td>
                        <td>
                            <ew:NumericBox ID="nbAgencySmallVan" runat="server" DecimalPlaces="0" PositiveNumber="True" /></td>
                    </tr>
                    <tr class="rgRow">
                        <td>
                            <asp:CheckBox ID="chkAgencyOther" runat="server" /></td>
                        <td>Other (please describe)
                            <asp:TextBox ID="txtAgencyOtherDesc" runat="server" Width="150px"></asp:TextBox></td>
                        <td>
                            <ew:NumericBox ID="nbAgencyOther" runat="server" DecimalPlaces="0" PositiveNumber="True" /></td>
                    </tr>
                </tbody>
            </table>
                
        </asp:Panel>
        <br />

        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlWellness" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkWellness" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepWellness" runat="server" Header="Wellness Services (See Section 1936, Chapter 19 of Title 29, DCMS, Wellness Services)" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgWellness" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
            
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </asp:Panel>
        <br />
        <div class="separatorBreak"></div>
        <asp:Panel ID="pnlOther" runat="server" Visible="false">
            <div>
                <asp:CheckBox ID="chkOther" runat="server" CssClass="chkBoxAlign" ViewStateMode="Enabled" />
                <uc1:Separator ID="sepOther" runat="server" Header="Other" Mode="1" />
            </div>
            <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgOther" runat="server" Width="100%" PageSize="50"
                OnUpdateCommand="gridServiceType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridServiceType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
              
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridServiceType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, REG_SPECIALTY_CATEGORY_ID, SPECIALTY_CATEGORY_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="50px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="50px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="50px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_NAME" HeaderText="Service Name" UniqueName="SPECIALTY_TYPE_NAME"   ReadOnly="true" HeaderStyle-Width="350px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" HeaderStyle-Width="100px"></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="85px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        <br />

             <div class="separatorBreak"></div>
            <telerik:RadGrid Skin="PDMSModern" EnableEmbeddedSkins="false"  ID="rgOtherCapacity" runat="server" Width="100%"
                OnUpdateCommand="gridEmployeeType_UpdateCommand"
                OnNeedDataSource="grid_NeedDataSource"
                
                OnDataBound="gridEmployeeType_DataBound" AllowMultiRowEdit="true" OnItemCreated="grid_ItemCreated" OnPreRender="grid_PreRender">
             
                <ValidationSettings EnableValidation="true" ValidationGroup="EPDWaiverValidation" CommandsToValidate="gridEmployeeType_ItemCommand" />
                <MasterTableView TableLayout="Fixed" AllowSorting="true" PageSize="50" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_ID, SPECIALTY_TYPE_ID, REG_SPECIALTY_ID, SPECIALTY_EMPLOYEE_TYPE_ID, REG_SPECIALTY_EMPLOYEE_ID, REG_SPECIALTY_APPROVAL_ID, SPECIALTY_SELECTED" CommandItemDisplay="None" EditMode="InPlace">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridCheckBoxColumn DataField="IsReviewerApproved" HeaderText="DDS Reviewer Approved Services" UniqueName="chkSpecialtyReviewerApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="IsOperatorApproved" HeaderText="DDS Operator Approved Services" UniqueName="chkSpecialtyOperatorApproved" Visible="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="SPECIALTY_SELECTED" HeaderText="Provider Requests" UniqueName="chkSpecialty" Display="true" HeaderStyle-Width="80px"></telerik:GridCheckBoxColumn>
                        <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID" HeaderText="" UniqueName="SPECIALTY_TYPE_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="Service Type Code" UniqueName="MMIS_SPECIALTY_TYPE_ID" HeaderStyle-Width="60px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMPLOYEE_TYPE_NAME" HeaderText="Employee Type" UniqueName="EMPLOYEE_TYPE_NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CAPACITY" HeaderText="Capacity" UniqueName="CAPACITY" MaxLength="3" HeaderStyle-Width="80px" HeaderTooltip="Please provide how many persons employed which will determine capacity"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="START_DATE" HeaderText="Begin Date" UniqueName="START_DATE" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="END_DATE" Display="false" UniqueName="END_DATE_OLD" ></telerik:GridBoundColumn>                        
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="END_DATE" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField formField" Text='<%#Eval("END_DATE", "{0:MM/dd/yyyy}") %>' />
                                <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtEndDate" runat="server" />

                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="EPDWaiverValidation"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate"
                                    ErrorMessage="Select a valid End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="PushButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="85px"></telerik:GridEditCommandColumn>
                    </Columns>
                     
                </MasterTableView>
                <ClientSettings>
                            <Resizing AllowColumnResize="True"></Resizing>
                         </ClientSettings>
            </telerik:RadGrid>
        </asp:Panel>
        <div class="separatorBreak"></div>
    </div>
    <br />
    <br />
</div>
<uc1:MessageBox ID="MessageBox2" runat="server" />
