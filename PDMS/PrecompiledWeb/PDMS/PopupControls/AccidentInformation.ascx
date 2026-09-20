<%@ control language="C#" autoeventwireup="true" inherits="UserControls_AccidentInformation, App_Web_yvhxe4ml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script type="text/javascript">
    function loaderAccedentInformation() {

        var value = $("#<%=ddlAccidentState.ClientID %>").val();
        if (value != "") {
            $("#<%=ddlAccidentcountry.ClientID %>").val('US');
        }
    }
    function loaderAccedentCountry() {
        var value = $("#<%=ddlAccidentcountry.ClientID %>").val();
        if (value != "US") {
            $("#<%=ddlAccidentState.ClientID %>").val('');
        }
    }

    function loaderAccedentrelatedto() {
        var value = $("#<%=ddlAccidentrelatedto.ClientID %>").val();
        if (value == "") {
            $("#<%=ddlAccidentrelatedto1.ClientID %>").prop("disabled", true);
            $("#<%=ddlAccidentrelatedto1.ClientID %>").empty();
        }
        else {
            var val = '';
            var param = '';
            $("#<%=ddlAccidentrelatedto1.ClientID %>").prop("disabled", false);
            $("#<%=ddlAccidentrelatedto1.ClientID %>").empty();
            if (value == "1") {
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=' + val + '>' + param + '</option>');
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=2>EM - Employment</option>');
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=3>OA - Other Accident</option>');
            }
            else if (value == "2") {
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=' + val + '>' + param + '</option>');
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=1>AA - Auto Accident</option>');
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=3>OA - Other Accident</option>');
            }
            else if (value == "3") {
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=' + val + '>' + param + '</option>');
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=1>AA - Auto Accident</option>');
                $('#<%=ddlAccidentrelatedto1.ClientID %>').append('<option value=2>EM - Employment</option>');
            }
        }
    }

    function validateDate1() {
        var daterequested = document.getElementById("<%= txtAccidentDate.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_birthDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_birthDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_txtAccidentDate').val('');

           }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_birthDateRequiredError1').css('display', 'none');

           }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_birthDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_birthDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_txtAccidentDate').val('');
        }

    }

</script>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        
        <ajax:CollapsiblePanelExtender ID="cpeAccidentinfo" runat="server" Collapsed="true" TargetControlID="pnlAccidentinfo" ExpandControlID="pnlsepAccidentinfo" CollapseControlID="pnlsepAccidentinfo" />
        <asp:Panel runat="server" ID="pnlsepAccidentinfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerAccidentinfo">
            <span id="sepAccidentinfo" runat="server" class="pageHeader pH2">+ ACCIDENT INFORMATION </span>
        </asp:Panel>
        <asp:Panel ID="pnlAccidentinfo" runat="server" Style="min-height: 290px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
            <div><asp:Label ID="lblErrorMsg2" runat="server" ForeColor="Red" Style="margin-left: 20px;"></asp:Label></div>

            <div id="dvAccRelatedTo" runat="server" class="col-sm-4">
                <div class="row" id="lblAccidentrelatedto" runat="server">
                    <div class="col-sm-5">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Accident Related To</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:HiddenField ID="hdnAccidentId" runat="server" />
                        <span style="text-align: left;">
                            <asp:DropDownList ID="ddlAccidentrelatedto" EnableViewState="true" runat="server" OnChange="loaderAccedentrelatedto()"
                                AppendDataBoundItems="True" Style="min-width: 190px; width:190px; height: 30px" AutoPostBack="false"
                                OnSelectedIndexChanged="ddlAccidentrelatedto_OnSelectedIndexChanged" class="selectdropdown" >
                            </asp:DropDownList>
                            <button id="btnloading2" runat="server" style="display:none;" CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                            <%--<asp:RequiredFieldValidator runat="server" ID="rfvAccidentrelatedto" SetFocusOnError="true"
                                ValidationGroup="valAccident" ControlToValidate="ddlAccidentrelatedto" Display="Dynamic"
                                ErrorMessage="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_ddlAccidentrelatedto' class='errorlist'>*Accident Related To is required</a></div>"
                                Text="*" />--%>

                        </span>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="StateCountryAJAX" runat="server">
                <ContentTemplate>

                    <div id="dvAccState" runat="server" class="col-sm-4">
                        <div class="row" id="lblAccidentState" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Accident State</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlAccidentState" OnChange="loaderAccedentInformation()"  EnableViewState="true" runat="server" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="ddlAccidentState_SelectedIndexChanged" Style="min-width: 190px; height: 30px" AutoPostBack="false">
                                    </asp:DropDownList>
                                    <button id="btnloading" runat="server" style="display:none;" CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                                    <%--<asp:CustomValidator runat="server" ID="cvState" ValidationGroup="valAccident" CssClass="failureNotification"
                                        ControlToValidator="ddlAccidentState" ErrorMessage="<div>*Accident State or Accident Country is required</div>" Text="*" 
                                        OnServerValidate="cvState_ServerValidate" />--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div id="dvAccCountry" runat="server" class="col-sm-4">
                        <div class="row" id="lblAccidentcounty" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Accident Country</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlAccidentcountry" EnableViewState="true" 
                                        runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlAccidentcountry_SelectedIndexChanged" AutoPostBack="false"
                                        Style="min-width: 190px; height: 30px"  OnChange="loaderAccedentCountry()">
                                    </asp:DropDownList>
                              
                                    <button id="btnAccidentCountry" runat="server" style="display:none;" CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                                    <asp:CustomValidator runat="server" ID="cvCountry" ValidationGroup="valAccident" CssClass="failureNotification"
                                        ControlToValidator="ddlAccidentcountry" ErrorMessage="" Text="*" 
                                        OnServerValidate="cvCountry_ServerValidate" />
                                </span>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="dvAccRelatedTo1" runat="server" class="col-sm-4">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Accident Related To</span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="ddlAccidentrelatedto1" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="min-width: 190px; height: 30px;"  Enabled="false" AutoPostBack="false">
                        </asp:DropDownList>
                       <%-- <asp:RequiredFieldValidator runat="server" ID="rfvAccidentrelatedto1" SetFocusOnError="true"
                            ValidationGroup="valAccident" ControlToValidate="ddlAccidentrelatedto1" Display="None"
                            ErrorMessage="*"
                            InitialValue="" Text="*Accident Related To is required" />--%>

                    </span>
                </div>
            </div>

            <div id="dvAccDate" runat="server" class="col-sm-4">
                <div class="row" id="lblAccidentDate" runat="server">
                    <div class="col-sm-5">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Accident Date</span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                      
                             <asp:TextBox ID="txtAccidentDate" runat="server" CssClass="formFieldcalender" Style="height: 30px; width: 180px" OnTextChanged="txtCheckForDate_TextChanged" AutoPostBack="false"  />
                             
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                                  
                            <ajax:CalendarExtender ID="ceAccidentDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtAccidentDate" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" PopupButtonID="imgBirthDate" EnabledOnClient="true" />
                          
                            <%-- <asp:CompareValidator ID="cvAccidentDate" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtAccidentDate" ValidationGroup="valAccident" ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"></asp:CompareValidator>
                            <asp:CompareValidator ID="cvtAccidentDate" runat="server" ErrorMessage="Accident date cannot be greater than today’s date”" Operator="LessThanEqual" ControlToValidate="txtAccidentDate" Type="Date" CssClass="failureNotification"></asp:CompareValidator>--%>
                           <%-- <asp:CustomValidator ID="cvAccidentDate1" runat="server" ControlToValidate="txtAccidentDate" ErrorMessage="Select a valid Accident Date." Display="Dynamic" Text="*" ValidationGroup="valAccident" OnServerValidate="ReportAccidentDate_ServerValidate" />
                             <asp:CustomValidator runat="server" ID="cvAccidentDateNotReported" ValidationGroup="valAccident" CssClass="failureNotification"
                                        ControlToValidator="txtAccidentDate" ErrorMessage="<div>*Accident date is required</div>" Text="<div>&nbsp;</div>" 
                                        OnServerValidate="cvDate_ServerValidate" />--%>
                            <asp:Label ID="birthDateRequiredError1" runat="server" Text="" CssClass="error-message" style="display:none;"></asp:Label>
                        </span>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:HiddenField ID="hdnClaimId" runat="server" />
<asp:HiddenField ID="hdnAccidentInfoClaimType" runat="server" />
