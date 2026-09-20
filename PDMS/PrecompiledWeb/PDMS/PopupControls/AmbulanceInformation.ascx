<%@ control language="C#" autoeventwireup="true" inherits="UserControls_AmbulanceInformation, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

<script type="text/javascript">

    function loaderddl1() {

        document.getElementById('<%= txtBoxPickupAddressLine1.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxPickupAddressLine2.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxPickUpCity.ClientID %>').disabled = true;
        document.getElementById('<%= ddlPickupState.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxPickUpZip.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxDropOffLocationName.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxDropOffAddressLine1.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxDropOffAddressLine2.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxDropOffCity.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDropOffState.ClientID %>').disabled = true;
        document.getElementById('<%= txtBoxDropOffZip.ClientID %>').disabled = true;
        document.getElementById('<%= txtPatientWeight.ClientID %>').disabled = true;
        document.getElementById('<%= ddlTransportReasonCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtTransportDistance.ClientID %>').disabled = true;
        document.getElementById('<%= txtRoundTripPurpose.ClientID %>').disabled = true;
        document.getElementById('<%= ddlConditionIndicator.ClientID %>').disabled = true;
        document.getElementById('<%= txtStretcherpurpose.ClientID %>').disabled = true;
        document.getElementById('<%= ddlConditionCode1.ClientID %>').disabled = true;
        document.getElementById('<%= ddlConditionCode2.ClientID %>').disabled = true;
        document.getElementById('<%= ddlConditionCode3.ClientID %>').disabled = true;
        document.getElementById('<%= ddlConditionCode4.ClientID %>').disabled = true;
        document.getElementById('<%= ddlConditionCode5.ClientID %>').disabled = true;
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
    }

    function onlyDotsAndNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            else
                return false;
        }

        if (txt.value.indexOf(".") > 0) {
            var txtlen = txt.value.length;
            var dotpos = txt.value.indexOf(".");
            //Change the number here to allow more decimal points than 2
            if ((txtlen - dotpos) > 2)
                return false;
        }

        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }
   
</script>
<asp:UpdatePanel ID="updatePaneAmbulanceInformation" runat="server">
    <ContentTemplate>
        <ajax:CollapsiblePanelExtender ID="cpeToothQuadrantInfo" runat="server" Collapsed="true" TargetControlID="pnlToothQuadrantInfo" ExpandControlID="pnlsepToothQuadrantInfo" CollapseControlID="pnlsepToothQuadrantInfo" />
        <asp:Panel runat="server" ID="pnlsepToothQuadrantInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerToothQuadrantInfo">
            <span id="sepToothQuadrantInfo" runat="server" class="pageHeader pH2">+ AMBULANCE INFORMATION </span>
        </asp:Panel>
        <asp:Panel ID="pnlToothQuadrantInfo" runat="server" Style="min-height: 140px; min-width: 150px; height: auto;  max-width: 100%; overflow-x: hidden;">
            <%-- <asp:ValidationSummary ID="valerrormess" DisplayMode="List" runat="server" CssClass="failureNotification"
             ValidationGroup="valAmbulanceInformation" />--%>
            <div>            
                <div class="row" id="divAmbulanceInformation" runat="server" style="background-color: lightblue; ">
                <div class="col-sm-4">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Pick-up Address Line 1</span>
                </div>
                <div class="col-sm-3">
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">Pick-up Address Line 2</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Pick-up City</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Pick-up State</span>
                </div>
                <div class="col-sm-1">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Pick-up Zip</span>
                </div>

            </div>

            <div class="row">
                <div class="col-sm-4">
                    <asp:TextBox runat="server" ID="txtBoxPickupAddressLine1" CssClass="formFieldTextBox" MaxLength="55" Style="height: 30px;" Width="375px" />
                    <asp:RequiredFieldValidator ID="rfvPickUpAddressLine1" ValidationGroup="valAmbulanceInformation" Display="Dynamic" CssClass ="failureNotification"
                        ControlToValidate="txtBoxPickupAddressLine1" runat="server"
                        Text="Ambulance pick-up address is required"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtBoxPickupAddressLine1" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>

                <div class="col-sm-3">
                    <asp:TextBox runat="server" ID="txtBoxPickupAddressLine2" CssClass="formFieldTextBox" MaxLength="55" Style="height: 30px;" />
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtBoxPickupAddressLine2" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
                <div class="col-sm-2">
                    <asp:TextBox runat="server" CssClass="formFieldTextBox" MaxLength="30" Style="height: 30px; width: 170px;" ID="txtBoxPickUpCity" />
                    <asp:RequiredFieldValidator ID="rfvPickUpCity" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxPickUpCity" runat="server"
                        Text="Ambulance pick-up city is required" CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtBoxPickUpCity" ID="revPickCity" ValidationGroup="valAmbulanceInformation" ForeColor="Red"
                        ValidationExpression="^[\s\S]{2,}$" runat="server" ErrorMessage="Pick Up address is invalid."></asp:RegularExpressionValidator>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBoxPickUpCity" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlPickupState" EnableViewState="true" runat="server"
                        AppendDataBoundItems="True" Style="height: 30px; width: 150px; min-width: 80px;" ValidationGroup="valAmbulanceInformation" class="selectdropdown">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvPickUpState" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="ddlPickupState" runat="server" 
                        Text="Ambulance pick-up state is required" 	CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-1">
                    <asp:TextBox runat="server" CssClass="formFieldTextBox" MaxLength="5" Style="height: 30px; width: 120px;" ID="txtBoxPickUpZip" />
                    <asp:RequiredFieldValidator ID="rfvPickUpZip" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxPickUpZip" runat="server"
                        Text="Ambulance pick-up zip is required" 	CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revZip" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxPickUpZip" runat="server" ErrorMessage="*"
                        Text="Pick Up ZIP code is invalid" CssClass ="failureNotification"
                        SetFocusOnError="True" ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="row" id="divAmbulancceInformationDropOff" runat="server" style="background-color: lightblue;">
                <div class="col-sm-2" >
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">Drop Off Location Name</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Drop Off Address Line 1</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">Drop Off Address Line 2</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Drop Off City</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Drop Off State</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left; font-weight: bold;">Drop Off Zip</span>
                </div>

            </div>
            <div class="row">
                <div class="col-sm-2">
                    <asp:TextBox runat="server" CssClass="formFieldTextBox" MaxLength="55" Style="height: 30px; width: 180px;" ID="txtBoxDropOffLocationName" />
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtBoxDropOffLocationName" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />

                </div>
                <div class="col-sm-2">
                    <asp:TextBox runat="server" CssClass="formFieldTextBox" MaxLength="55" Style="height: 30px; width: 180px;" ID="txtBoxDropOffAddressLine1" ValidationGroup="valAmbulanceInformation" />
                    <asp:RequiredFieldValidator ID="rfvTxtDropOffAddressLine1" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxDropOffAddressLine1" runat="server" 
                        Text="Ambulance drop-off address is required" 	CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtBoxDropOffAddressLine1" ValidationExpression="^[A-Za-z0-9?\#\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
                <div class="col-sm-2">
                    <asp:TextBox runat="server" CssClass="formFieldTextBox" MaxLength="55" Style="height: 30px; width: 180px;" ID="txtBoxDropOffAddressLine2" />
                    <asp:RequiredFieldValidator ID="rfvDropOffAddressLine2" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxDropOffAddressLine2" runat="server" ForeColor="Red"
                        Text="Ambulance drop-off Address Line 2 is required" CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtBoxDropOffAddressLine2" ValidationExpression="^[A-Za-z0-9?\#\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
                <div class="col-sm-2">
                    <asp:TextBox runat="server" CssClass="formFieldTextBox" MaxLength="30" Style="height: 30px; width: 180px;" ID="txtBoxDropOffCity" ValidationGroup="valAmbulanceInformation" />
                    <asp:RequiredFieldValidator ID="rfvDropOffcity" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxDropOffCity" runat="server"  ForeColor="Red"
                        Text="Ambulance drop-off city is required" CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtBoxDropOffCity" ForeColor="Red" ID="revDropCity" ValidationGroup="valAmbulanceInformation"
                        ValidationExpression="^[\s\S]{2,}$" runat="server" ErrorMessage="Drop-off address is invalid."></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtBoxDropOffCity" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
									
                </div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlDropOffState" EnableViewState="true" runat="server"
                        AppendDataBoundItems="True" Style="height: 30px; width: 150px; min-width: 80px;" ValidationGroup="valAmbulanceInformation" class="selectdropdown">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDropState" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="ddlDropOffState" runat="server" ForeColor="Red"
                        Text="Ambulance drop-off state is required" CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="ddlDropOffState" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />

                </div>
                <div class="col-sm-2">
                    <asp:TextBox runat="server" CssClass="formFieldTextBox" MaxLength="10" Style="height: 30px; width: 180px;" ID="txtBoxDropOffZip" ValidationGroup="valAmbulanceInformation" />
                    <asp:RequiredFieldValidator ID="rfvDropOffZip" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxDropOffZip" runat="server" 
                        Text="Ambulance drop-off zip is required" CssClass ="failureNotification"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revTxtBoxDropZip" ValidationGroup="valAmbulanceInformation" Display="Dynamic"
                        ControlToValidate="txtBoxDropOffZip" runat="server" ErrorMessage="*"
                        Text="Drop-off ZIP code is invalid" CssClass ="failureNotification"
                        SetFocusOnError="True" ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                </div>

            </div>
            <div class="col-sm-12 row" id="divTransportation" runat="server" style="background-color: lightblue; margin-left: 0px">
                <div class="col-sm-12">
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">Transport Information</span>
                </div>
            </div>
            <div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="lblPatientWeight" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Patient Weight (LB):</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtPatientWeight" runat="server" CssClass="formFieldTextBox"  MaxLength="10"  Style="height: 30px; width: 210px" ValidationGroup="valAmbulanceInformation" onkeypress="return onlyDotsAndNumbers(this,event);" />
                                    
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtPatientWeight" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                             ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />


                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Transport Reason Code</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlTransportReasonCode" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="height: 30px; width: 50px; min-width: 80px; width: 180px" ValidationGroup="valAmbulanceInformation" class="selectdropdown">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTransportCode" runat="server" ControlToValidate="ddlTransportReasonCode" CssClass ="failureNotification"
                                         Text="*Transportation reason code is required" Display="Dynamic" ValidationGroup="valAmbulanceInformation">
                                    </asp:RequiredFieldValidator>

                                </span>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="Div2" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Transport Distance (Miles):</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtTransportDistance" runat="server" CssClass="formFieldTextBox" MaxLength="15" Style="height: 30px; width: 210px" ValidationGroup="valAmbulanceInformation" onkeypress="return onlyDotsAndNumbers(this,event);" />
                                    <asp:RequiredFieldValidator ID="rfvTransportDistance" runat="server" ControlToValidate="txtTransportDistance" CssClass ="failureNotification"
                                        Text="*Transport distance is required" Display="Dynamic" ValidationGroup="valAmbulanceInformation">
                                    </asp:RequiredFieldValidator>
                                </span>
                                
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtTransportDistance" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div3" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Round trip Purpose:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtRoundTripPurpose" runat="server" CssClass="formFieldTextBox" MaxLength="80" Style="height: 30px; width: 210px" ValidationGroup="valAmbulanceInformation" />

                                </span>
                                
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txtRoundTripPurpose" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="Div4" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Condition Indicator:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlConditionIndicator" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="height: 30px; width: 180px; min-width: 80px;" ValidationGroup="valAmbulanceInformation">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="No"></asp:ListItem>
                                    </asp:DropDownList>

                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div5" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Stretcher Purpose:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtStretcherpurpose" runat="server" CssClass="formFieldTextBox" MaxLength="80" Style="height: 30px; width: 210px" ValidationGroup="valAmbulanceInformation" />

                                </span>
                                
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txtStretcherpurpose" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="Div6" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Condition Code:</span>
                            </div>
                            <div class="col-sm-7">
                              <%--  <span style="text-align: left;">--%>
                                    <div class="row">
                                        <span>
                                            <asp:DropDownList ID="ddlConditionCode1" EnableViewState="true" runat="server" OnChange="loaderddl1()"
                                                AppendDataBoundItems="True" Style="height: 30px; width: 50px; min-width: 80px;" ValidationGroup="valAmbulanceInformation"
                                                OnSelectedIndexChanged="ddlConditionCode1_OnSelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList></span>
                                        <span>
                                            <asp:DropDownList ID="ddlConditionCode2" EnableViewState="true" runat="server" OnChange="loaderddl1()"
                                                AppendDataBoundItems="True" Style="height: 30px; width: 50px; min-width: 80px;" ValidationGroup="valAmbulanceInformation"
                                                OnSelectedIndexChanged="ddlConditionCode2_OnSelectedIndexChanged" AutoPostBack="true" Enabled="false">
                                            </asp:DropDownList></span>

                                        <span>
                                            <asp:DropDownList ID="ddlConditionCode3" EnableViewState="true" runat="server" OnChange="loaderddl1()"
                                                AppendDataBoundItems="True" Style="height: 30px; width: 50px; min-width: 80px;" ValidationGroup="valAmbulanceInformation"
                                                OnSelectedIndexChanged="ddlConditionCode3_OnSelectedIndexChanged" AutoPostBack="true" Enabled="false">
                                            </asp:DropDownList></span>

                                        <span>
                                            <asp:DropDownList ID="ddlConditionCode4" EnableViewState="true" runat="server" OnChange="loaderddl1()"
                                                AppendDataBoundItems="True" Style="height: 30px; width: 50px; min-width: 80px;" ValidationGroup="valAmbulanceInformation"
                                                OnSelectedIndexChanged="ddlConditionCode4_OnSelectedIndexChanged" AutoPostBack="true" Enabled="false">
                                            </asp:DropDownList></span>

                                        <span>
                                            <asp:DropDownList ID="ddlConditionCode5" EnableViewState="true" runat="server" OnChange="loaderddl1()"
                                                AppendDataBoundItems="True" Style="height: 30px; width: 50px; min-width: 80px;" ValidationGroup="valAmbulanceInformation"
                                                AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlConditionCode5_SelectedIndexChanged">
                                            </asp:DropDownList></span>



                                    </div>
                                <div>  <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     </div>

                                <%--</span>--%>
                            </div>
                        </div>
                    </div>


                </div>
            </div>
                </div>

       

            <div>
                <div runat="server" id="divConfirmAddress" style="display: none; text-align: center">
                    <p style="color: darkgreen">
                        According to the USPS database, the address entered is inaccurate. The following address was found:
                    </p>
                    <p style="color: darkgreen" id="paraUSPSAddress" runat="server"></p>
                    <p style="color: darkgreen">Click on 'Accept' to accept the corrections.</p>
                    <br />
                    <asp:Button ID="btnConfirmAddress" CssClass="buttonBoxFocus" Text="Accept" runat="server" />
                    <asp:Button ID="btnCancelAddressCorrection" CssClass="buttonBox" Text="Cancel" runat="server" />
                </div>

                <div runat="server" id="divWSError" style="display: none; text-align: left">
                    <p style="color: darkgreen">
                        An error occurred while validating the entered address.<br />
                        You can continue to work, but any addresses will not be validated by the system.<br />
                        <br />
                        Error details:<br />
                    </p>
                    <p style="color: darkgreen" id="paraWSError" runat="server"></p>
                    <asp:Button ID="btnConfirmWSError" class="buttonBox" Text="Ok" runat="server" />
                </div>
            </div>

            <asp:CustomValidator ID="cvAddress"
                ControlToValidate=""
                OnServerValidate="cvAmbulanceServiceAddress_ServerValidate"
                Display="None"
                ErrorMessage=""
                ValidationGroup="valAmbulanceInformation"
                runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hdnClaimId" runat="server" />
<script type="text/javascript">
    function maxZIndex() {
        var highest = -999;

        $("*").each(function () {
            var current = parseInt($(this).css("z-index"), 10);
            if (current && highest < current)
                highest = current;
        });

        return highest;
    }

    jQuery.expr[':'].contains = function (a, i, m) {
        return jQuery(a).text().toUpperCase()
            .indexOf(m[3].toUpperCase()) >= 0;
    };

    function sendDataAmbulanceInformation(newStreetAddress, newUnitAddress, floorDept, newAddressLine3, newCity, newState, newCounty, newZip5) {
        $("#<%= txtBoxDropOffAddressLine1.ClientID %>").first().val(newStreetAddress);
        $("#<%= txtBoxDropOffAddressLine2.ClientID %>").first().val(newUnitAddress);
        $("#<%= txtBoxDropOffCity.ClientID %>").first().val(newCity);
        if ($("#<%= ddlDropOffState.ClientID %>").first().val() !== newState) {
            $("#<%= ddlDropOffState.ClientID %>").first().val(newState);
            $("#<%= ddlDropOffState.ClientID %>").change();
        }
        $('#<%= ddlDropOffState.ClientID %> option:contains(' + newState + ')').attr("selected", "selected");

       <%-- $("#<%= ddlCounty.ClientID %> option").filter(function () {
            return $(this).text().toUpperCase() === county.toUpperCase();
        }).prop('selected', true);--%>
<%--        $('#<%= ddlCounty.ClientID %> option:selected').text(county);--%>      


        $("#<%= txtBoxDropOffZip.ClientID %>").first().val(newZip5);
    }

    function allowSave() {
        $("#<%= SaveButtonClientID %>").prop("disabled", false);
    }

    function setAddressConfirm(confirmValue) {
        var hdnAddressConfirm = document.getElementById("<% = hdnAddressConfirm.ClientID %>");
        hdnAddressConfirm.value = confirmValue;
    }
</script>
<asp:HiddenField ID="hdnAddressConfirm" runat="server" Value="0" />
<asp:HiddenField ID="hdnSaveButtonClientID" runat="server" Value="" />
<asp:HiddenField ID="hdnAmbulanceInfoNo" runat="server" Value="" />

