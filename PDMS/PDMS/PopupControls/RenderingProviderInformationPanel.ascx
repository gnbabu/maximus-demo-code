<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_RenderingProviderInformationPanel" Codebehind="RenderingProviderInformationPanel.ascx.cs" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script type="text/javascript">
    function loaderRenderingProvider(e) {
        getTextboxdetail(e);
        $("#<%= lblRenderingErrorMessage.ClientID %>").text('');
        var renderingNpi = $("#<%= txtRenderingProvNPI.ClientID %>").val();
        var renderingnpiLenth = renderingNpi.length;
        if ((renderingnpiLenth > 0) && (renderingnpiLenth < 10))
        {
            RenderingClerFields();
            $("#<%= lblRenderingErrorMessage.ClientID %>").text("10 digit NPI is required");
            localStorage.setItem("RenderingProviderNPI", "");
            return false;
        }
        localStorage.setItem("RenderingProviderNPI", "" + renderingNpi + "");
        var referprovNpi = localStorage.getItem("ReferingProviderNPI");
        var PrimaryprovNpi = localStorage.getItem("PrimarycareReferingProviderNPI");
        var AsstSurNpi = localStorage.getItem("AsstProviderNPI");
       
        if (!(renderingNpi === null || renderingNpi === undefined || renderingNpi === "" || renderingNpi.length == 0) && !(referprovNpi === null || referprovNpi === undefined || referprovNpi === "" || referprovNpi.length == 0) && renderingNpi === referprovNpi) {
            $("#<%= lblRenderingErrorMessage.ClientID %>").text("Rendering Provider cannot be same as Referring Provider");
            RenderingClerFields();
            return false;
        }
        else if (!(renderingNpi === null || renderingNpi === undefined || renderingNpi === "" || renderingNpi.length == 0) && !(PrimaryprovNpi === null || PrimaryprovNpi === undefined || PrimaryprovNpi === "" || PrimaryprovNpi.length == 0) && renderingNpi === PrimaryprovNpi) {
            $("#<%= lblRenderingErrorMessage.ClientID %>").text("Rendering provider cannot be same as primary provider");
            RenderingClerFields();
            return false;
        }
        else if (!(renderingNpi === null || renderingNpi === undefined || renderingNpi === "" || renderingNpi.length == 0) && !(AsstSurNpi === null || AsstSurNpi === undefined || AsstSurNpi === "" || AsstSurNpi.length == 0) && renderingNpi === AsstSurNpi) {
            if ($("#<%= hdnClaimType_Rendering.ClientID %>").first().val() == "0" ) {
                $("#<%= lblRenderingErrorMessage.ClientID %>").text("Rendering Provider and Assistant Provider provider both cannot be entered in the same claim");
                RenderingClerFields();
                return false;
            }
            if ($("#<%= hdnClaimType_Rendering.ClientID %>").first().val() == "1" ) {
                $("#<%= lblRenderingErrorMessage.ClientID %>").text("Rendering Provider and Other Physician provider both cannot be entered in the same claim");
                RenderingClerFields();
                return false;
            }
        }
      
        else if ($("#<%= hdnBillingNPI.ClientID %>").val() === renderingNpi) {
            $("#<%= lblRenderingErrorMessage.ClientID %>").text("Rendering provider ID should only be entered if it is different than billing provider ID");
            RenderingClerFields();
            return false;
        }

        else if (renderingnpiLenth == 10 && renderingnpiLenth !== null && renderingnpiLenth !== undefined) {
            var txtrendNpiCode = $("#<%= txtRenderingProvNPI.ClientID %>").first().val();
            var txtrendMedId = $("#<%= hdnRendMedId.ClientID %>").first().val();
            var hdnRenderingProviderClaimType = $("#<%= hdnClaimType_Rendering.ClientID %>").first().val();
            $("#<%= lblRenderingErrorMessage.ClientID %>").html("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetnpiCount?RefNpi=" + txtrendNpiCode + "&&RefMedId=" + txtrendMedId + "&&ClaimType=" + hdnRenderingProviderClaimType + "",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result === "") {
                        $("#<%= lblRenderingErrorMessage.ClientID %>").text("NPI is not found in the system");
                        RenderingClerFields();
                        return false;
                    }
                    else {
                        $("#<%= lblRenderingErrorMessage.ClientID %>").removeAttr("disabled");
                        if (result == "NPI is not found in the system") {
                            $("#<%= lblRenderingErrorMessage.ClientID %>").text("NPI is not found in the system");
                            RenderingClerFields();
                            return false;
                        }
                        else if (result == "10-digit number is required") {

                            $("#<%= lblRenderingErrorMessage.ClientID %>").text("10 - digit number is required");
                            RenderingClerFields();
                            return false;
                        }
                      
                        else if (result !== null && result !== undefined && result !== "NPI is not found in the system" && result !== "10 - digit number is required") {
                            var RenderingMedicaidID = (result[0]["MEDICAID_ID"]);
                            localStorage.setItem("RenderingMedicaidID", RenderingMedicaidID);
                            $("#<%= lblRenderingFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                            $("#<%= lblRenderingLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);
                            $("#<%= hdnRendMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                            $("#<%= hdnRendFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                            $("#<%= hdnRendLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
                            var medid = result[0]["MEDICAID_ID"];
                            if (medid !== null && medid !== undefined) {
                                $("#<%= lblRenderingErrorMessage.ClientID %>").text("");
                                var APIToken = $("[id*=hdnAccessToken]").val();
                                $.ajax({
                                    type: "GET",
                                    url: webApiClaims + "GetnpiCount?RefNpi=" + txtrendNpiCode + "&&RefMedId=" + medid + "&&ClaimType=" + hdnRenderingProviderClaimType + "",
                                    headers: {
                                        "Access-Control-Allow-Origin": "*",
                                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                        "Authorization": "Bearer " + APIToken
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        if (result == "Non-individual provider cannot be entered as referring provider") {
                                            if (hdnRenderingProviderClaimType == "1") {
                                                $("#<%= lblRenderingErrorMessage.ClientID %>").text("Non-individual provider cannot be entered as rendering provider");
                                                RenderingClerFields();
                                                return false;
                                            }
                                        }
                                        else {
                                            $("#<%= lblRenderingMedicaidID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                                            $("#<%= lblRenderingFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                                            $("#<%= lblRenderingLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                                            $("#<%= hdnRendMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                                            $("#<%= hdnRendFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                                            $("#<%= hdnRendLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
                                        }
                                    },
                                });
                            }
                            return false;
                        }
                    }
                },
            });
        }
        else if (renderingnpiLenth == 0) {
            $("#<%= lblRenderingMedicaidID.ClientID %>").html('');
            var RenderingMedicaidID = $("#<%= lblRenderingMedicaidID.ClientID %>").html();
            localStorage.setItem("RenderingMedicaidID", RenderingMedicaidID);
            $("#<%= lblRenderingFirstName.ClientID %>").html('');
            $("#<%= lblRenderingLastName.ClientID %>").html('');

            $("#<%= lblRenderingMedicaidID.ClientID %>").text('');
            $("#<%= lblRenderingFirstName.ClientID %>").text('');
            $("#<%= lblRenderingLastName.ClientID %>").text('');

            $("#<%= hdnRendMedId.ClientID %>").val('');
            $("#<%= hdnRendFirstName.ClientID %>").val('');
            $("#<%= hdnRendLastName.ClientID %>").val('');
        }
        return false;
    }
    function RenderingClerFields() {
        localStorage.setItem("RenderingProviderNPI", "");
        localStorage.setItem("RenderingMedicaidID", "");
        $("#<%= txtRenderingProvNPI.ClientID %>").val("");
        $("#<%= lblRenderingMedicaidID.ClientID %>").text('');       
        $("#<%= lblRenderingLastName.ClientID %>").text('');        
        $("#<%= lblRenderingFirstName.ClientID %>").text('');
        $("#<%= hdnRendMedId.ClientID %>").val('');
        $("#<%= hdnRendFirstName.ClientID %>").val('');
        $("#<%= hdnRendLastName.ClientID %>").val('');

    }
   

</script>
<asp:UpdatePanel ID="upnlRenderingProvider" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hdnClaimIdRendering" runat="server" />
        <asp:HiddenField ID="hdnClaimType_Rendering" runat="server" />
        <asp:HiddenField ID="hdnBillingNPI" runat="server" />
        <asp:HiddenField ID="hdnErrorMessageRender" runat="server" />
        <asp:HiddenField ID="hdnRendMedId" runat="server" />
        <asp:HiddenField ID="hdnRendFirstName" runat="server" />
        <asp:HiddenField ID="hdnRendLastName" runat="server" />

        <div class="col-sm-12 row" id="divRenderingTitle" runat="server" style="background-color: lightblue;
            margin-left: 0px">
            <div class="col-md-3">
                <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left;
                    font-weight: bold; padding-left: 110px;">NPI</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;
                    padding-left: 70px;">Medicaid ID</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;
                    padding-left: 30px;">Last Name</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">
                    First Name</span>
            </div>
        </div>
        <div class="row" style="padding-left: 100px;">
            <div class="col-sm-3">
                <div class="row" id="lblRenderingProvNPI" runat="server">                   
                    <div class="col-sm-12">
                        <span style="text-align: left;">                        

                                <asp:TextBox ID="txtRenderingProvNPI" runat="server" CssClass="formFieldTextBox" MaxLength="10"
                                    Style="height: 30px; width: 200px;" OnChange="return loaderRenderingProvider(this);" 
                                    AutoPostBack="false" />
                              <asp:Label ID="Searchd" runat="server">
                                <button type="button" id="btnSearch7" onclick="getbuttondetail(this)" class="btn btn-link"
                                    data-toggle="modal" data-target="#myModal">
                                    Search
                                </button>
                            </asp:Label>
                            
                            <br />
                          <%--  <asp:RequiredFieldValidator ID="rfvRenderingProvNPI" runat="server" ControlToValidate="txtRenderingProvNPI"
                                ErrorMessage="<div id='msg8_txtRenderingProvNPI' ><a href='#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI' class='errorlist'> *Rendering Provider NPI 10-digit number is required.</a></div>"
                                Display="None" ValidationGroup="validate" CssClass="failureNotification"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="revRefNPI"
                                Display="None" CssClass="failureNotification"
                                ValidationGroup="validateRenderingInformation"
                                ControlToValidate="txtRenderingProvNPI"
                                ValidationExpression="^[0-9]{10}$"
                                ErrorMessage="<div id='msg8_rev_txtRenderingProvNPI'>Rendering Provider NPI 10-digit number is required</div>">	
                            </asp:RegularExpressionValidator>--%>
                        </span>
                        <asp:Label ID="errRenderingProviderNPI" Style="color: red" runat="server"></asp:Label>
                         <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="lblRenderingProvMedicaidID" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: justify; display: none;">
                            <asp:Label ID="lblRenderingMedicaidID" runat="server" Style="font-size: 20px; padding-left: 40px;" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="lblRenderingProviderLastName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: justify;">
                            <asp:Label ID="lblRenderingLastName" runat="server" Style="font-size: 20px; padding-left: 50px;" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="lblRenderingProvFirstName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: justify;">
                            <asp:Label ID="lblRenderingFirstName" runat="server" Style="font-size: 20px; padding-left: 70px;" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div id="divRenderingErrorMessage" class="row" runat="server">
            <div class="col-sm-12">
                <asp:Label ID="lblRenderingErrorMessage" runat="server" Text="" CssClass="failureNotification"></asp:Label>
            </div>
        </div>
    </ContentTemplate>

</asp:UpdatePanel>

<%--<ajax:ModalPopupExtender BehaviorID="mpeNPIRendSearchPopup" ID="mpeRenderSubmitClaimSearchProc" runat="server" PopupControlID="pnlRendSubmitClaimSearchPop"
    TargetControlID="ButtonRend9" BackgroundCssClass="modalBackground" CancelControlID="btnCloseCH9" />

<asp:Panel ID="pnlRendSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 1px; min-width: 800px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchPopHeader"  runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseCH9"  Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
            CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left:50px">NPI</span>
            <span style="padding-left:60px">MEDICAID ID</span>
            <span style="padding-left:50px">BUSINESS/LAST NAME</span>
            <span style="padding-left:60px">FIRST NAME</span>
        </div>
    </asp:Panel>
   
</asp:Panel>
<asp:Button runat="server" ID="ButtonRend9" Style="display: none" Text="ButtonRend9" />--%>
 

