<%@ page title="Services Referral Entry" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_DIDDReferralDataEntry, App_Web_qtcaivva" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <asp:Label id="lblPageTitle" runat="server" Text="Services Referral Data Entry" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script  type="text/javascript">
        function numericOnly(obj) {
            obj.value = obj.value.replace(/[^0-9]/g, '');
        }
        
        function RadioCheck(rb,divpnlServices) {
            
         var gv = document.getElementById("<%=grdProviderNPITaxIDMatches.ClientID%>");
         var rbs = gv.getElementsByTagName("input");
         var idx = 0;
         var row = rb.parentNode.parentNode;
         for (var i = 0; i < rbs.length; i++) {
             if (rbs[i].type == "radio") {
                 if (rbs[i].checked && rbs[i] != rb) {
                     rbs[i].checked = false;
                     break;
                 }
                 if (rbs[i] == rb) {
                     idx = i;
                 }
             }
         }

     }    

    </script>    
    <br />
    
    <div>
        <div style="display: inline-block">
            <asp:ValidationSummary ID="vsDIDDReferral" DisplayMode="List" runat="server" CssClass="failureNotification"
                ValidationGroup="valReferral" />
            <asp:Label runat="server" ID="lblMessage" Text="" CssClass="bodyTextRed"></asp:Label>
            <table>
                <tr id="tr_Action" runat="server">
                    <td colspan="4" style="text-align: center;padding-bottom:20px;"><span class="formLabel">Action</span>
                        <asp:DropDownList ID="ddlAction" runat="server" CssClass="formFieldAuto" MaxLength="100" 
                            OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel170">Provider First Name</td>
                    <td><asp:TextBox ID="txtFirstName" runat="server" CssClass="formField" MaxLength="50" TabIndex="1" /></td>
                    <td class="formLabel170">Provider Last Name</td>
                    <td><asp:TextBox ID="txtLastName" runat="server" CssClass="formField" MaxLength="50" TabIndex="2" /></td>
                </tr>
                <tr>
                    <td  class="center">-OR-</td>
                </tr>
                <tr>
                    <td class="formLabel170">Group/Entity Name</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtGroupEntityName" runat="server" CssClass="formField300" MaxLength="100" TabIndex="3" />
                        <asp:CustomValidator ID="valNameRqd" runat="server" ErrorMessage="*Enter First/Last Name or Group/Entity Name"
                            Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valReferral"></asp:CustomValidator>
                        <asp:CustomValidator ID="valNameNotBoth" runat="server" ErrorMessage="*Enter First/Last Name -OR- Group/Entity Name but not both"
                            Text="*" Display="Dynamic"  SetFocusOnError="true" ValidationGroup="valReferral"></asp:CustomValidator>
                    </td>
                </tr>
                <tr><td colspan="4" style="height:10px;"></td></tr>
                <tr>
                    <td><span class="formLabel170">Tax ID*</span></td>
                    <td>
                        <asp:TextBox ID="nbTaxID" runat="server" CssClass="formField" MaxLength="9" TabIndex="5" onKeyUp="javascript:numericOnly(this);" />
                        <asp:RequiredFieldValidator runat="server" ID="valTaxIDReqd"  ControlToValidate="nbTaxID" ErrorMessage="* Enter Tax ID" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valReferral" />
                        <asp:RegularExpressionValidator ID="valTaxIDFormat" runat="server" ControlToValidate="nbTaxID"  
                            ValidationExpression="(?!0{9})(?!9{9})^\d{9}$" ErrorMessage="* Tax ID requires 9 digits" Text="*" Display="Dynamic" 
                            ValidationGroup="valReferral" />
                    </td>
                     <td><span class="formLabel170"><asp:Literal ID="ltlAppNbr" runat="server" Text =" <%$ Resources:BrandingResource , WAIVER_SERVICES_APPLICATION_NUMBER_SHORT %>" /></span></td>
                    <td>
                        <asp:TextBox ID="txtApplicationNumber" runat="server" CssClass="formFieldReadOnly" MaxLength="20"  ReadOnly="true" />
                        <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                            ControlToValidate="txtApplicationNumber" ErrorMessage="* Enter Referral Number" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valReferral" />--%>
                    </td>
               </tr>

                <tr >
                    <td><span class="formLabel170">Provider Email</span></td>
                    <td colspan="2">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="formField220" MaxLength="100" TabIndex="7" />
<%--                        <asp:RequiredFieldValidator runat="server" ID="valEmailReqd"
                            ControlToValidate="txtEmail" ErrorMessage="* Enter Provider Email" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valReferral" />--%>
                        <asp:RegularExpressionValidator runat="server" ID="valEmailFormat"
                            ControlToValidate="txtEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ErrorMessage="* Enter valid email address" Text="*" Display="Dynamic" SetFocusOnError="true"
                            ValidationGroup="valReferral" />
                    </td>
                    <td><%--<span class="formLabel170">NPI (if applicable)</span>--%></td>
                    <td><%--
                        <asp:TextBox ID="txtNPI" runat="server" CssClass="formField" MaxLength="10" TabIndex="6" onKeyUp="javascript:numericOnly(this);" />
                        <asp:RegularExpressionValidator ID="valNPINbrFormat" runat="server" ControlToValidate="txtNPI"  
                            ValidationExpression="^\d{10}$" ErrorMessage="* Enter a 10-digit NPI" Text="*" Display="Dynamic" 
                            ValidationGroup="valReferral" />--%>
                    </td>
                </tr>

                <tr>
                    <td><span class="formLabel170">Provider Zip*</span></td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server" CssClass="formFieldMedium" MaxLength="5" TabIndex="7"  onKeyUp="javascript:numericOnly(this);"/>
                        <asp:RequiredFieldValidator runat="server" ID="valZipReqd" ControlToValidate="txtZip" ErrorMessage="* Enter Provider Zip Code" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valReferral" />
                        <asp:RegularExpressionValidator runat="server" ID="rvZip" ControlToValidate="txtZip" ErrorMessage="*Enter 5 digit Zip" ValidationExpression="^(?!0{5})(?!9{5})\d{5}$" ValidationGroup="valReferral" Text="*"/>
                        
                        
                        
                         - <asp:TextBox ID="txtZipExt" runat="server" CssClass="formFieldSmall" MaxLength="4" TabIndex="7"  onKeyUp="javascript:numericOnly(this);" />
                        
                        <asp:RequiredFieldValidator runat="server" ID="valZipExtReqd" ControlToValidate="txtZipExt" ErrorMessage="* Enter Provider Zip Code Extension" Text="*" Display="Dynamic"  SetFocusOnError="true" ValidationGroup="valReferral" />
                        
                        
                        <asp:RegularExpressionValidator runat="server" ID="rvZipExt" ControlToValidate="txtZipExt" ErrorMessage="*Enter 4 digit Zip Extension" ValidationExpression="^\d{4}$" ValidationGroup="valReferral" Text="*"/>
                    </td>

                </tr>
            </table>
            <table>
                <tr style="vertical-align:text-top;">
                    <td class="formLabel170"> Select the correct option:</td>
                    <td> <asp:RadioButtonList ID="rblSubmitType" runat="server" RepeatDirection="Vertical"  style="padding:0; margin:0;" >
                            <asp:ListItem Selected="False" Text="Provider will submit paper application" Value="1"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="Provider will submit electronic enrollment form" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel170">Services Provider In/By:*</td>
                    <td class="fieldValue"><asp:DropDownList ID="ddlLocation" runat="server"   AutoPostBack="false" />            
                        <asp:CompareValidator runat="server" ID="valLocCmp" ControlToValidate="ddlLocation"  ValueToCompare="0" Type="Integer" ErrorMessage="* Select a value for Services Provider In/By" 
                            Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="valReferral" />
                    </td>
                </tr>
            </table>
            <br />
            
            <div>
                <span class="formLabel170" ><asp:Literal ID="ltlAssistedLiving" runat="server" Text ="Assisted Living" />*</span>
                <asp:CheckBox ID="chkAssistedLiving" runat="server" AutoPostBack="true" OnCheckedChanged="chkAssistedLiving_CheckedChanged" />
            </div>
            <div class="wd170 center" >-OR-</div>
            <div style="display:inline;">
                   <span class="formLabel170" ><asp:Literal ID="ltlWaiverType" runat="server" Text =" <%$ Resources:BrandingResource , WAIVER_TYPE_NAME %>" />*</span>
                            <asp:CheckBoxList ID="chklstWaiver" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="chklstWaiver_SelectedIndexChanged" AutoPostBack="true">
                            </asp:CheckBoxList>
                            <asp:CustomValidator runat="server" ID="cvWaiver" ErrorMessage="* Please select a program type" Text="*" ValidationGroup="valReferral"> 
                            </asp:CustomValidator>
            </div>
        </div>
    <br />
    <br />
        <div style="display: none; padding: 5px">
                        <span class="formLabel150">Service From Date*:</span>
                        <ajax:CalendarExtender 
                            ID="ceFromDate" 
                            runat="server" 
                            Format="MM/dd/yyyy"  
                            TargetControlID="txtContractFromDate" 
                            PopupPosition="BottomRight"  
                            PopupButtonID="imgFromDate"  
                            EnabledOnClient="true" />
                        <asp:TextBox ID="txtContractFromDate" runat="server" CssClass="formField130" />
                                  <%--  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                            ControlToValidate="txtContractFromDate" ErrorMessage="* Service from Date required" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valReferral" />--%>

                        <span class="formLabel150">Service To Date*:</span>
                        <ajax:CalendarExtender 
                            ID="ceToDate" 
                            runat="server" 
                            Format="MM/dd/yyyy"  
                            TargetControlID="txtContractToDate" 
                            PopupPosition="BottomRight"  
                            PopupButtonID="imgFromDate"  
                            EnabledOnClient="true" />
                        <asp:TextBox ID="txtContractToDate" runat="server" CssClass="formField130" />
                                  <%--  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4"
                            ControlToValidate="txtContractToDate" ErrorMessage="* Service to Date required" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valReferral" />--%>
        </div>
        <br />

            <div style="text-align: center; padding-top:4px; width:95%;">
                <div style="display: inline-block; ">
                    <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" Text="Submit" CssClass="buttonBox" TabIndex="14" onclick="btnSubmit_Click" 
                        ValidationGroup="valReferral" />&nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" TabIndex="15"   CssClass="buttonBox" onclick="btnClear_Click" />
                </div>
            </div>
        </div>

    <div style="text-align: center;padding-top:10px">
        <div style="display: inline-block">
           <asp:GridView runat="server" ID="grdServicesAD" AutoGenerateColumns="False" HorizontalAlign="Left" Caption="AD Services" 
                CssClass="gridview" EmptyDataText="No AD services found." Width="700" DataKeyNames="DIDD_SERVICE_ID" GridLines="Both" ShowHeader ="true">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkServiceID" runat="server"/>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="WAIVER_TYPE_CODE" ItemStyle-Width="200px"  HeaderText="Service Type Code"/>
                    <asp:BoundField DataField="NAME" ItemStyle-Width="450px" HeaderText="Service Name" />
                </Columns>
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>

            <br />
            <asp:GridView runat="server" ID="grdServicesCDD" AutoGenerateColumns="False" HorizontalAlign="Left" Caption="CDD Services" 
                CssClass="gridview" EmptyDataText="No CDD services found." Width="700" DataKeyNames="DIDD_SERVICE_ID" GridLines="Both"  ShowHeader ="true">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkServiceID" runat="server"/>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="WAIVER_TYPE_CODE" ItemStyle-Width="200px"  HeaderText="Service Type Code" />
                    <asp:BoundField DataField="NAME" ItemStyle-Width="450px" HeaderText="Service Name"/>
                </Columns>
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>

            <br />
            <asp:GridView runat="server" ID="grdServicesPASS" AutoGenerateColumns="False" HorizontalAlign="Left" Caption="PAS Services" 
                CssClass="gridview" EmptyDataText="No PAS services found." Width="700" DataKeyNames="DIDD_SERVICE_ID" GridLines="Both" ShowHeader ="true">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkServiceID" runat="server"/>                           
                        </ItemTemplate>
                         <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="WAIVER_TYPE_CODE" ItemStyle-Width="200px" HeaderText="Service Type Code"/>
                    <asp:BoundField DataField="NAME" ItemStyle-Width="450" HeaderText="Service Name" />
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>


            <br />
            <asp:GridView runat="server" ID="grdServicesDDAC" AutoGenerateColumns="False" HorizontalAlign="Left" Caption="DDAC Services" 
                CssClass="gridview" EmptyDataText="No DDAC services found." Width="700" DataKeyNames="DIDD_SERVICE_ID" GridLines="Both" ShowHeader ="true">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkServiceID" runat="server"/>                           
                        </ItemTemplate>
                         <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="WAIVER_TYPE_CODE" ItemStyle-Width="200px" HeaderText="Service Type Code"/>
                    <asp:BoundField DataField="NAME" ItemStyle-Width="450" HeaderText="Service Name" />
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>


            <asp:GridView runat="server" ID="grdServicesDDAD" AutoGenerateColumns="False" HorizontalAlign="Left" Caption="DDAD Services" 
                CssClass="gridview" EmptyDataText="No DDAD services found." Width="700" DataKeyNames="DIDD_SERVICE_ID" GridLines="Both" ShowHeader ="true">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkServiceID" runat="server"/>                           
                        </ItemTemplate>
                         <ItemStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="WAIVER_TYPE_CODE" ItemStyle-Width="200px" HeaderText="Service Type Code"/>
                    <asp:BoundField DataField="NAME" ItemStyle-Width="450" HeaderText="Service Name" />
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </div>

    <uc:MessageBox ID="ucMessageBox" runat="server"   />

    <!-- ModalPopupExtender -->
    <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
        CancelControlID="btnCancel" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" align="left" style="display:none;">
        <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" >
            <div class="popTitle">Confirmation</div>
        </asp:Panel>  
        <asp:Panel ID="pnlMain" runat="server" Style="padding:10px; width: 400px">
            <asp:Label ID="lblPopupMessage" runat="server" />
        </asp:Panel>
        <div class="btnBox">
            <asp:Button id="btnYes"  runat="server" Text="Yes, save and continue" CssClass="buttonBox" onclick="btnYes_Click" CausesValidation="true" />
            <asp:Button id="btnCancel"  runat="server" Text="No, cancel and return to page" CssClass="buttonBox" CausesValidation="false" />
        </div> 
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
    <asp:Button runat="server" ID="btnDummy" Style="display: none" text="btnDummy"/>
    <asp:HiddenField ID="hdnDIDD_REFERRAL_TYPE_ID" runat="server" />

    <ajax:ModalPopupExtender ID="mpeConfirm" runat="server" PopupControlID="pnlConfirm" TargetControlID="btnDummy"
        CancelControlID="btnCancelConfirm" BackgroundCssClass="modalBackground" >
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlConfirm" runat="server" CssClass="modalPopup" align="center">        
        <asp:Panel ID="Panel1" CssClass="popHeader" runat="server" HorizontalAlign="Left">
            <div style="text-align:left">&nbsp;&nbsp;
                <asp:Label ID="Label2" CssClass="bodyTextBold" runat="server" Text="Other Referrals with same Tax ID" ForeColor="White" />
            </div>
        </asp:Panel> 
                        <%-- <asp:UpdatePanel ID="updiddReferral" runat="server"  UpdateMode="Conditional" style="margin-left:auto;margin-right:auto;">
                    <ContentTemplate>--%>
        <div  style="overflow-y:scroll; height:800px;">
        <%--<br />
        <div style="text-align:left;font-weight:bold;padding-left:1.5em">
           <asp:Label ID="Label1" runat="server" Text="List of providers matching NPI/Tax ID you entered"></asp:Label>
        </div> --%>
        <br /> 
        <div style="text-align:left;padding-left:2em">
            <asp:Label runat="server" ID="lblErrorMessage" CssClass="failureNotificationMessage"></asp:Label>
        </div>
        <br />
        <div style="text-align:left">&nbsp;&nbsp;
                <asp:Label ID="Label1" CssClass="bodyTextRed" runat="server" Text="Based on the Tax Id you entered, the following Referrals are available in the system."  />
            <br />
        </div>
        <asp:GridView runat="server" ID="grdProviderNPITaxIDMatches" BorderStyle="Solid" AutogenerateColumns="false" OnRowDataBound="grdProviderNPITaxIDMatches_RowDataBound"  DataKeyNames="didd_referral_id">
            <Columns>

                                <asp:TemplateField>
                    <ItemTemplate>
       
                        <asp:RadioButton runat="server" ID="rdoProvider" GroupName="SelectProvider" onclick ="RadioCheck(this);" OnCheckedChanged="rdoProvider_CheckedChanged" AutoPostBack="true"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OrganizationName" HeaderText="Organization Name" ItemStyle-Width="100" />
                <asp:BoundField DataField="TaxID" HeaderText="Tax ID"  ItemStyle-Width="100" />
                <asp:BoundField DataField="ApplicationNo" HeaderText="Referral No" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="MedicaidID" HeaderText="OrganizationID" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Servicing_Zip" HeaderText="Zip" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="PDMSStatus" HeaderText="Review Status"  HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="PDMSStatusDate" HeaderText="Status Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"  HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="ReferredBy" HeaderText="Referred By"  HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To"  HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Waivers" HeaderText="Program"  HeaderStyle-HorizontalAlign="Left" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <tr style="margin-left:20px;" class="divChild">
                                                                                        <td colspan="100%"><div class="childGrid" id="pnlServices" runat="server" style="margin-left:50px;">
                                <asp:GridView runat="server" ID="grdServices" AutoGenerateColumns="False" 
                                    DataKeyNames="DIDD_REFERRAL_SERVICE_ID, CONTRACT_FROMDATE"
                                    CssClass="gridViewSmallFont" EmptyDataText="No services found."
                                    >       
                                    <Columns>
                                    <asp:BoundField DataField="WAIVER_TYPE_CODE" HeaderText="Service Type Code" ItemStyle-Width="180" />
                                    <asp:BoundField DataField="NAME" HeaderText="Service Name"  ItemStyle-Width="180"/>
                        <asp:BoundField DataField="CONTRACT_FROMDATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="CONTRACT_FROMDATE" ItemStyle-Width="180" />
                        <asp:BoundField DataField="CONTRACT_TODATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="CONTRACT_TODATE" ItemStyle-Width="180"  />
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
                                </div></td>
                        </tr>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
      <%--  </asp:Panel>--%>
        <br />
        <table>
            <tr><td>            <asp:Label ID="lblReferralStatus" runat="server" Text="" CssClass="bodyTextRed"/></td></tr>
        </table>
        <table border="0" style="padding:0;border-collapse:separate;border-spacing:5px;text-align:right;padding-bottom: 10px" role="presentation">           
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>                
                <td>
                    <asp:Button id="btnUseSelectedProvider"  runat="server" Text="Use Selected Referral" CssClass="buttonBox" OnClick="btnSubmitSelected_Click"/>
                </td>
                <td>
                    <asp:Button id="btnCancelConfirm"  runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" OnClick="btnCancelConfirm_Click"/>
                </td>
                                <td>
                    <asp:Button id="btnDoNotUseAnyOfThese"  runat="server" Text="Save As Is" CssClass="buttonBox" OnClick="btnDoNotUseAnyOfThese_Click"/>
                </td>

            </tr>

        </table>
        <table>
             <tr>
                 <td>
                    <div class="field-hint">
                        <ul>
                        <li>"Use Selected Referral" will not create a new Referral</li>
                            <li>"Cancel" will take you back to the Services Referral Data Entry</li>
                            <li>"Save As Is" will create a new referral with the given data in Services Referral Data Entry Page</li>
                            </ul>
                    </div>
                </td>
            </tr>
        </table>

            </div>
                                            <!--</ContentTemplate>
                </asp:UpdatePanel>-->
    </asp:Panel>
</asp:Content>
