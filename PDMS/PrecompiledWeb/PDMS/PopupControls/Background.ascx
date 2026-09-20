<%@ control language="C#" autoeventwireup="true" inherits="Pages_Background, App_Web_yvhxe4ml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>

<style type="text/css">
    /*.formField wd250, .formDropDown {
        width:250px !important;
    }*/
    select {
        min-width: 250px !important;
    }
</style>
<div id="divFCBCInfo">
    <asp:ValidationSummary ID="valBackground" DisplayMode="List" runat="server" CssClass="failureNotification val-summary" ValidationGroup="BackgroundValidation" Visible="true" Enabled="true" ShowSummary="true" />
    <uc:Separator ID="Separator1" runat="server" Header="Fingerprint and Background Check Information" />
    <br />
    <div class="row divGrid">
        <div class="col-sm-12 center">
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    function RowDblClick(sender, eventArgs) {
                        sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
                    }

                    function checkDate(sender, args) {
                        if (sender._selectedDate > new Date()) {
                            sender._selectedDate = null;
                            sender._textbox.set_Value(null)
                            document.getElementById("errormessage").style.display = "block";
                        }
                        else {
                            document.getElementById("errormessage").style.display = "none";
                        }
                    }
            
                </script>
            </telerik:RadCodeBlock>

            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
            </telerik:RadAjaxLoadingPanel>
            <telerik:RadGrid RenderMode="Lightweight" ID="rgFCBC" runat="server" Width="100%"
                OnUpdateCommand="rgFCBC_UpdateCommand"
                OnNeedDataSource="rgFCBC_NeedDataSource" OnItemCreated="rgFCBC_ItemCreated" OnItemCommand="rgFCBC_ItemCommand" OnItemDataBound="rgFCBC_ItemDataBound" EnableEmbeddedSkins="false" Skin="PDMSModern">
                <HeaderStyle CssClass="gridViewHeader" />
                <PagerStyle Mode="NumericPages" CssClass="gridViewPager" />
                <ItemStyle CssClass="gridViewRow" />
                <AlternatingItemStyle CssClass="gridViewAltRow" />
                <SelectedItemStyle CssClass="gridViewSelected" />
                <ValidationSettings EnableValidation="true" ValidationGroup="BackgroundValidation" CommandsToValidate="rgFCBC_ItemCommand,rgFCBC_UpdateCommand" />
                <MasterTableView AllowSorting="true" PageSize="15" AllowPaging="True" Width="100%" AutoGenerateColumns="false" DataKeyNames="REG_BACKGROUND_CHECK_ID,REG_ID,REG_OWNER_ID,NAME" CommandItemDisplay="None">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="REG_OWNER_ID" HeaderText="REG_OWNER_ID" UniqueName="REG_OWNER_ID" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NAME" HeaderText="Name" UniqueName="NAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FBINoticeIssued" HeaderText="FBI Notice Issued" UniqueName="FBINoticeIssued"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NoticeDate" HeaderText="Initial Notice Date" UniqueName="Notice_Date"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BACKGROUND_DATE" HeaderText="Final Disposition Date" UniqueName="FBI_BACKGROUND_DATE" DataFormatString="{0:MM/dd/yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BACKGROUND_RESULT_TYPE" HeaderText="Disposition" UniqueName="Disposition"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BACKGROUND_STATUS_TYPE" HeaderText="Background Check Complete" UniqueName="BACKGROUND_STATUS_TYPE" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsBackgroundCheckRequired" HeaderText="IsBackgroundCheckRequired" UniqueName="IsBackgroundCheckRequired" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="VersionHistory" HeaderText="VersionHistory" UniqueName="VersionHistory" Display="false"></telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="BACKGROUND_DATE" HeaderText="Date Background Check Complete" UniqueName="BACKGROUND_DATE" DataFormatString="{0:MM/dd/yyyy}"></telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="BACKGROUND_RESULT_TYPE" HeaderText="Result" UniqueName="BACKGROUND_RESULT_TYPE" Display="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BACKGROUND_PERFORMED_BY_TYPE" HeaderText="Performed By" UniqueName="BACKGROUND_PERFORMED_BY_TYPE"></telerik:GridBoundColumn>

                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="ImageButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" EditImageUrl="~/Images/edit.png" UpdateImageUrl="../App_Themes/Default/Grid/Update.gif" CancelImageUrl="../App_Themes/Default/Grid/Cancel.gif">
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <div id="Table2" style="border-collapse: collapse;">
                                <div class="row">
                                    <div class="col-sm-9">
                                        <b>Background Check Details</b>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:Label AssociatedControlID="rblBackgroundCheckReq" ID="lbBbackgroundCheckReq" runat="server" CssClass="formLabel200" Text="Is Background Check Required?" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:RadioButtonList ID="rblBackgroundCheckReq" runat="server" SelectedValue='<%# Bind("IsBackgroundCheckRequired") %>' OnDataBinding="PollDistribPointsTypeOptions_DataBinding" OnSelectedIndexChanged="PollDistribPointsTypeOptions_SelectedIndexChanged" CssClass="QstRadioList" TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true">
                                            <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="pnlBKCNotRequired" Enabled="false" Visible="false">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <asp:Label AssociatedControlID="ddlReasonBackgroundCheckNotReq" ID="lblReasonBackgroundCheckNotReq" runat="server" CssClass="formLabel200" Text="Reason Background Check not Required:" />
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlReasonBackgroundCheckNotReq" runat="server" CssClass="formField wd250" OnInit="ddlReasonBackgroundNotReq_Init" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("ReasonBackgroundCheckNotReq") %>'>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rgvReasonBackgroundCheckNotReq" runat="server" ControlToValidate="ddlReasonBackgroundCheckNotReq" Enabled="true" SetFocusOnError="true"
                                                Display="Dynamic" Text="*" ValidationGroup="valBackground" ErrorMessage="* Reason Background Check Required."></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <asp:Panel runat="server" ID="pnlBKCRequired" Enabled="false" Visible="false">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblOwnerNamelbl" AssociatedControlID="lblOwnerName" runat="server" Text="Name" CssClass="formLabel200" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblOwnerName"  runat="server" Text='<%# Bind("NAME") %>'></asp:Label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblBackgroundCheckComplete" AssociatedControlID="ddlBackgroundComplete" runat="server" Text="Background Check Complete" CssClass="formLabel200" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlBackgroundComplete" runat="server" CssClass="formField wd250" OnInit="ddlBackgroundComplete_Init" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("BACKGROUND_STATUS_TYPE_ID") %>'>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblDateBackgroundCheckComplete" AssociatedControlID="txtBackgroundDate" runat="server" Text="Date Background Check Complete" CssClass="formLabel200" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtBackgroundDate" runat="server" CssClass="formField wd250"  Text='<%# Bind("BACKGROUND_DATE", "{0:MM/dd/yyyy}") %>' />
                                            <span id="errormessage" style="display:none; color:red">Future date not allowed</span>
                                            <ajax:CalendarExtender ID="calBackgroundDate" TargetControlID="txtBackgroundDate" runat="server" Format="MM/dd/yyyy"  OnClientDateSelectionChanged="checkDate" />
                                              
                                            <asp:CompareValidator ID="cvBackgroundDate" runat="server" ValidationGroup="valBackground"
                                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBackgroundDate"
                                                ErrorMessage="Select a valid Background Check Date" Text="*"  Display="Dynamic"   ValueToCompare="MM/dd/yyyy"
                                   
                                                SetFocusOnError="true" />
                                         
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblResult" AssociatedControlID="ddlBackgroundResult" runat="server" Text="Result" CssClass="formLabel200" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlBackgroundResult" runat="server" AutoPostBack ="true" CssClass="formField wd250" OnInit="ddlBackgroundResult_Init" OnSelectedIndexChanged ="ddlBackgroundResult_IndexChanged" OnDataBound ="ddlBackgroundResult_DataBound" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("BACKGROUND_RESULT_TYPE_ID") %>'>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblPerformedBy" AssociatedControlID="ddlBackgroundPerformedBy" runat="server" Text="Performed By" CssClass="formLabel200" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlBackgroundPerformedBy" runat="server" CssClass="formField wd250" OnInit="ddlBackgroundPerformedBy_Init" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("BACKGROUND_PERFORMED_BY_TYPE_ID") %>'>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <asp:Label AssociatedControlID="rblIssueFBILetter" ID="lblIssueFBILetter" runat="server" CssClass="formLabel200" Text="Issue FBI Letter?" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:RadioButtonList ID="rblIssueFBILetter" runat="server" SelectedValue='<%# Bind("IssueFBILetter") %>' CssClass="QstRadioList" TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true">
                                                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label AssociatedControlID="rblEnrolledInRapBack" ID="lblEnrolledInRapBack" runat="server" CssClass="formLabel200" Text="Enrolled in Rap Back?" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:RadioButtonList ID="rblEnrolledInRapBack" runat="server" SelectedValue='<%# Bind("IsEnrolledRapBack") %>' CssClass="QstRadioList" TextAlign="Right" RepeatDirection="Horizontal" AutoPostBack="true">
                                                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 text-center">
                                            <asp:Button ID="btnPoorQualityFingerPrints" runat="server" Text="Poor Quality Prints" CssClass="buttonBoxFocus" OnClick="btnPoorQualityFingerPrints_Click"/>
                                            <asp:Button ID="btnOrderBackgroundCheckWithoutFingerprints" runat="server" Text="Order Background Check Without Fingerprints" CssClass="buttonBoxFocus" />
                                            <asp:Button ID="btnRejectBCReport" runat="server" Text="Reject BC Report" CssClass="buttonBoxFocus" OnClick="btnRejectBCReport"/>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:Label AssociatedControlID="txtComments" ID="Label11" runat="server" CssClass="formLabel200" Text="Comments:" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="formField wd250" Text='<%# Bind("Comments") %>'></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-9"></div>
                                    <div class="col-sm-3 text-right">
                                        <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' 
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBoxFocus"></asp:Button>&nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" CssClass="buttonBox"></asp:Button>
                                    </div>
                                </div>

                            </div>

                        </FormTemplate>
                    </EditFormSettings>

                </MasterTableView>
                <ClientSettings>
                    <ClientEvents OnRowDblClick="RowDblClick"></ClientEvents>
                </ClientSettings>
            </telerik:RadGrid>
        </div>
    </div>
</div>
