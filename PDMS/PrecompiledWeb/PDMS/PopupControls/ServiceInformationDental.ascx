<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ServiceInformationDental, App_Web_wbqq1lcm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchPop.ascx" TagPrefix="uc" TagName="SubmitClaimSearchPop" %>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
        <meta name="viewport" content="width=device-width, initial-scale=1">

 <script>
     function ddlDentalReleaseofInfoChanged() {

         var ddlDentalReleaseofInfo = $("#<%=ddlDentalReleaseofInfo.ClientID %> option:selected").text();

         if ((ddlDentalReleaseofInfo != "") && (ddlDentalReleaseofInfo != null)) {
             $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_releaseOfInformationDentalError').text('');
             $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_ddlDentalReleaseofInfo').css("background-color", "white");
         }
     }

     function ValidationForPlceofService()
     {
         ddlDentalReleaseofInfoChanged();
         
         var txtPlaceofService = document.getElementById("<%=txtPlaceofService.ClientID %>").value;
         if ((txtPlaceofService != "") && (txtPlaceofService != null)) {

             $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_placeOfInformationError').text('');
         }
         if (txtPlaceofService.length === 2) {
             var hdnClaimId = document.getElementById("<%=hdnClaimId.ClientID %>").value;
             var APIToken = $("[id*=hdnAccessToken]").val();
             $.ajax({
                 type: "GET",
                 url: webApiPA + "GetPlaceOfServiceCodeDetails?desc=" + "" + "&&val=" + txtPlaceofService,
                 //data: '{desc: "" , val: "' + txtPlaceofService + '" }',
                 headers: {
                     "Access-Control-Allow-Origin": "*",
                     "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                     "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                     "Authorization": "Bearer " + APIToken
                 },
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (result) {
                     if ((result.length == 0) || (result.length > 1)) {
                         $("[id*=placeOfInformationError]").text('* Place of Service Code is invalid');
                         return false;
                     }
                     else {
                         $("[id*=placeOfInformationError]").text('');
                     }
                 },
                 error: function (jqXHR, textStatus, errorThrown) {
                     $("[id*=placeOfInformationError]").text('Place of Service code not found.');
                     return false;
                 }

             });
         } else {
             $("[id*=placeOfInformationError]").text('* Place of Service Code is invalid');
             return false;
         }
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

   
function visiblePlaceOfService() {
        $find("mpePlaceOfService").show();
        $("#lblSubmitClaimSearchPop").text("Place Of Service Search");
        return false;
    }
    
        function alphanumericOnly(obj) {
            obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
     }

     function visiblePlaceOfServiceCodeServicedetail() {
         $("#<%= ucSubmitClaimSearchPop.FindControl("txtCode").ClientID %>").first().val("");
         $("#<%= ucSubmitClaimSearchPop.FindControl("txtPlaceOfServiceName").ClientID %>").first().val("");
         $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("");
         $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        localStorage.setItem("indexPlaceOfServicecodeserviceDetailPanel", "true");
         $find("mpePlaceOfService").show();
        return false;
    }

     function GetPlaceOfServiceCodeInfoDentalPlaceOfService() {
         $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("");
         $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
         var txtPlaceCode = $("#<%= ucSubmitClaimSearchPop.FindControl("txtCode").ClientID %>").first().val();
         var txtPiacecodedesc = $("#<%= ucSubmitClaimSearchPop.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();
         if (txtPlaceCode == "" && txtPiacecodedesc == "") {
             $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("Place of service code or name is required");
             return false;
         }
         if (txtPlaceCode.length >= 1) { 
         if (txtPlaceCode != undefined && txtPlaceCode.length != 2 && txtPlaceCode.length < 2) {
             $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("*2-Digit place of service code is required");
              return false;
         }
         }
         var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiPA + "GetPlaceOfServiceCodeDetails?desc=" + txtPiacecodedesc + "&&val=" + txtPlaceCode,
             //data: '{desc: "' + txtPiacecodedesc + '" , val: "' + txtPlaceCode + '" }',
             headers: {
                 "Access-Control-Allow-Origin": "*",
                 "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                 "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                 "Authorization": "Bearer " + APIToken
             },
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (result) {
                 if (result.length == 0) {

                     $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                }
                else {
                    $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><th>Place Of Service Code </th><th>Place Of Service Code Description </th></tr>");
                     for (var i = 0; i < result.length; i++) {
                         
                         
                         $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td><a onClick='GetPlaceOfserviceCodeDentalServiceDetails(this); return false;'>" + result[i].Placeofservice_Code + "</asp:LinkButton></td><td>" + result[i].Placeofservice_Desc + "</td></tr>");

                     };
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblmessage]").text('Place Of Service code not found.');
             }
         });
         
          return false;
    }

     function GetPlaceOfserviceCodeDentalServiceDetails(lnk) {
        $find("mpePlaceOfService").hide();
        var gridindexProccode = localStorage.getItem("indexPlaceOfServicecodeserviceDetailPanel");
        var textboxrow = lnk.parentNode.parentNode;
        $("[id*=txtPlaceofService]").val(textboxrow.cells[0].innerText.trim());
        return false;
     }

     $(document).ready(function () {
         $("#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_txtPlaceofService").on("input", function () {
             ValidationForPlceofService();
         });
     });
 </script>

<asp:UpdatePanel ID="upICD10ProcedureCodes" runat="server">
    <ContentTemplate>
         <div>
                    <asp:Label ID="lblErrorMsg1" runat="server" ForeColor="Red" Style="margin-left: 20px;"></asp:Label>
                </div>
        <div class="row">
            <div class="col-md-4">
                <div class="row" id="lblSpecialProgramCode" runat="server">
                        <div class="col-sm-5">
                            <span class="ohio-field" style="font-size: 15px; text-align: right">Special Program Code</span>
                        </div>
                        <div class="col-sm-7">
                           <span style="text-align: right;">
                                <asp:DropDownList ID="ddlEPSDTCondition" EnableViewState="true" runat="server"
                                     AppendDataBoundItems="True" Style="height: 30px; width: 200px;">
                                </asp:DropDownList>
                           </span>
                        </div>
                </div>
            </div>
            <div class="col-md-4">
               <div class="row" id="lblPatientAmountPaid" runat="server">
                    <div class="col-sm-5">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Patient Amount Paid</span>
                    </div>
                <div class="col-sm-7">
                    <span style="text-align: right;">
                        <asp:TextBox ID="txtPatientAmountPaid" runat="server" onkeypress="return onlyDotsAndNumbers(this,event);"  MaxLength="18" CssClass="formFieldTextBox" Style="height: 30px; width: 200px" />
                        <asp:CompareValidator ID="cvPatientAmountPaid" runat="server" ControlToValidate="txtPatientAmountPaid" 
                             Operator="GreaterThan" Type="Integer" ValueToCompare="0" />
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtPatientAmountPaid" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                    </span>
                </div>
            </div>
            </div>
            <div class="col-md-4">
               <div class="row" id="DivDOS" runat="server">
                    <div class="col-sm-5">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Date Of Service</span>
                    </div>
                <div class="col-sm-7">
                    <span style="text-align: right;">
                       <asp:Label ID="lblServiceDateInformation" Style="font-size: 14px;" runat="server" Text=""></asp:Label>
                    </span>
                </div>
            </div>
            </div>
        </div>
        <div class="row">
           <div class="col-md-4">
                <div class="row" id="lblDentalReleaseofInfo" runat="server">
                    <div class="col-sm-5">
                        <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>Release of Information</span>
                    </div>
                    <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="ddlDentalReleaseofInfo" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 90px;" OnChange="ddlDentalReleaseofInfoChanged()" class="selectdropdown">
                            <asp:ListItem Value="0" Text=""></asp:ListItem>                           
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="N" Text="No" />
                        </asp:DropDownList>
                        <asp:Label ID="releaseOfInformationDentalError"  runat="server" Text="" CssClass="error-message"></asp:Label>
                       <%--<asp:RequiredFieldValidator runat="server" ID="rfvDentalReleaseofInfo" SetFocusOnError="true"
                            ValidationGroup="validateClaims" ControlToValidate="ddlDentalReleaseofInfo"  ForeColor="Red"
                           ErrorMessage="<div id='msg3_ddlDentalReleaseofInfo'><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_ddlDentalReleaseofInfo' class='errorlist'>*Release of Information is required</a></div>"  Display="None" InitialValue="0" />--%>
                    </span>
                </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="row" id="lblPlaceofService" runat="server">
                    <div class="col-sm-5">
                        <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>Place of Service</span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: right;">
                            <asp:TextBox ID="txtPlaceofService"  onChange="ValidationForPlceofService()" runat="server" CssClass="formFieldPos" MaxLength="2" Style="height: 30px; width: 130px"  onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);" AutoComplete="off"/> 
                           
                    
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtPlaceofService" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            <asp:LinkButton ID="lnkPlaceofServiceSearch" Text="Search" runat="server" ToolTip="Search" OnClientClick="return visiblePlaceOfServiceCodeServicedetail()" Visible="true" CausesValidation="false"></asp:LinkButton>    
                            <asp:Label ID="placeOfInformationError" runat="server"  Text="" CssClass="error-message"></asp:Label>
                           <%-- <asp:RequiredFieldValidator ID="rfvPlaceOfService" runat="server" ControlToValidate="txtPlaceofService" InitialValue="" ForeColor="Red"
                                            ErrorMessage="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_txtPlaceofService' class='errorlist'>*Place of Service is required</a></div>" Text="*" Display="None" ValidationGroup="validateClaims"></asp:RequiredFieldValidator>--%>
                            <br/>
                             <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
   
                            <asp:Label runat="server" ID="lblServiceInfoDentalErr" ForeColor="Red"></asp:Label>
                        
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                 <div class="row" id="DivPCI" runat="server">
                    <div class="col-sm-5">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Predetermination Claim ID</span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: right;">
                            <asp:TextBox ID="txtPreClaimID" runat="server" CssClass="formFieldTextBox" onKeyUp="javascript:alphanumericOnly(this);"  MaxLength="50" Style="height: 30px; width:100px" />
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPreClaimID" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                        </span>
                    </div>
                </div>
            </div>
            
           
           
           
     <asp:HiddenField id="hdnClaimId" runat="server" />
     <asp:HiddenField id="hdnStartDate" runat="server" />
     <asp:HiddenField id="hdnEndDate" runat="server" />
     <asp:HiddenField id="hdnDateOfService" runat="server" />

        </div>
   
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- DenatalSearch --%>
    <asp:Panel runat="server" ID="pnlSepPlaceofServiceSearch" class="CollapsingSeparator" Visible="false" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerDiagnosis">

        <span id="SepPlaceofServiceSearch" runat="server" class="pageHeader pH2">Search</span>
    </asp:Panel>
    <asp:Panel ID="pnlPlaceofServiceSearch" Visible="false" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
        <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
            <div class="row" style="text-align: center;">
                <div class="col-sm-6 col-md-4 col-lg-3 ">
                    <span class="ohio-field-label"><b>Code</b>
                        <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server">
                        </asp:TextBox>

                    </span>

                </div>

                <div class="col-sm-6 col-md-4 col-lg-3 ">
                    <span class="ohio-field-label"><b>Place of Service Name</b>
                        <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" runat="server">
                        </asp:TextBox><asp:LinkButton ID="lnkPLaceServiceName" Text="Search" runat="server" ToolTip="Search" OnClick="LinkButton8_Click" Visible="true">

                        </asp:LinkButton>
                        
                    </span>


                </div>
            </div>
            <div class="divPlaceofServiceSearch" style="padding-top: 10px">
                <asp:GridView ID="gvPlaceofServiceSearch" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
                    EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdPlaceofServiceSearch_SelectedIndexChanged" Style="margin-right: 100px">

                    <Columns>
                        <asp:BoundField DataField="Code" HeaderText="Code" />
                        <asp:BoundField DataField="Placeofname" HeaderText="Place Of Service Name" />

                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
 <ajax:ModalPopupExtender BehaviorID="mpePlaceOfService" ID="mpeSubmitClaimSearchPop" runat="server" PopupControlID="pnlSubmitClaimSearchPop"
        TargetControlID="Button9" BackgroundCssClass="modalBackground" CancelControlID="btnCloseCH9" />
    
    <asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
        <asp:Panel ID="pnlSubmitClaimSearchPopHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
            <asp:Button runat="server" ID="btnCloseCH9" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" />
            <div class="search-Results">
<span style="padding-left:10px;font-size:17px;">CODE</span>
<span style="padding-left:250px;font-size:17px;">PLACE OF SERVICE</span>
               </div>
        </asp:Panel>
        <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
            <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:SubmitClaimSearchPop runat="server" ID="ucSubmitClaimSearchPop" Visible="true" EnableViewState="true" />
                        </div>
                    </div>
            
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />