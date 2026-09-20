<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_AssistantSurgeon" Codebehind="AssistantSurgeon.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">
    function loaderAssistantSurgeonDental(e) {
        getTextboxdetail(e);
        $("#<%= errAssistantSurgeonNPI.ClientID %>").text('');
        
        var Asstnpi = $("#<%= txtAssistantSurgeonNPI.ClientID %>").val();
        var AsstnpiLenth = Asstnpi.length;
        if ((AsstnpiLenth > 0) && (AsstnpiLenth < 10)) {
            AsstClerFields();
            $("#<%= errAssistantSurgeonNPI.ClientID %>").text("10 digit NPI is required");
            localStorage.setItem("AsstProviderNPI", "");
            return false;
        }
        localStorage.setItem("AsstProviderNPI", "" + Asstnpi + "");
        var renderingprovnpi = localStorage.getItem("RenderingProviderNPI"); 
        var supervisingprov = localStorage.getItem("SuperVisingProviderNPI");

        if (!(renderingprovnpi === null || renderingprovnpi === undefined || renderingprovnpi === "" || renderingprovnpi.length == 0) && !(Asstnpi === null || Asstnpi === undefined || Asstnpi === "" || Asstnpi.length == 0) && Asstnpi === renderingprovnpi) {
            $("#<%= errAssistantSurgeonNPI.ClientID %>").text("Rendering and Assistant surgeon provider both cannot be entered in the same claim");
            AsstClerFields();
            return false;
        }
        else if (!(supervisingprov === null || supervisingprov === undefined || supervisingprov === "" || supervisingprov.length == 0) && !(Asstnpi === null || Asstnpi === undefined || Asstnpi === "" || Asstnpi.length == 0) && Asstnpi === supervisingprov) {
            if ($("#<%= hdnAssistantSurgeonClaimType.ClientID %>").first().val() === "0" || $("#<%= hdnAssistantSurgeonClaimType.ClientID %>").first().val() === "2") {
                $("#<%= errAssistantSurgeonNPI.ClientID %>").text("Assistant Surgeon cannot be same as Supervising Provider");
                AsstClerFields();
               
            }
            if ($("#<%= hdnAssistantSurgeonClaimType.ClientID %>").first().val() === "1") {
                $("#<%= errAssistantSurgeonNPI.ClientID %>").text("Other Operating Physician cannot be same as Operating Physician Provider");
                AsstClerFields();
            }
            return false;
           }
        else if (AsstnpiLenth == 10 && AsstnpiLenth !== null && AsstnpiLenth !== undefined) {
            var txtrefNpiCode = $("#<%= txtAssistantSurgeonNPI.ClientID %>").first().val();
            var txtrefMedId = $("#<%= hdnAssistSurMedId.ClientID %>").first().val();

            var hdnAsstProviderClaimType = $("#<%= hdnAssistantSurgeonClaimType.ClientID %>").first().val();
            $("#<%= errAssistantSurgeonNPI.ClientID %>").text("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetAsstnpiCount?AsstNpi=" + txtrefNpiCode + "&&AsstMedId=" + txtrefMedId + "&&ClaimType=" + hdnAsstProviderClaimType+"", 
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
                        $("#<%= errAssistantSurgeonNPI.ClientID %>").text("NPI is not found in the system");
                        AsstClerFields();
                        return false;
                    }
                    else {
                        if (result == "NPI is not found in the system") {
                            $("#<%= errAssistantSurgeonNPI.ClientID %>").text("NPI is not found in the system");
                            AsstClerFields();
                            return false;
                        }
                        else if (result == "10-digit number is required") {
                            $("#<%= errAssistantSurgeonNPI.ClientID %>").text("10 - digit number is required");
                            AsstClerFields();
                            return false;
                        }                      
                        else if (result !== null && result !== undefined && result !== "NPI is not found in the system" && result !== "10 - digit number is required") {
                            $("#<%= errAssistantSurgeonNPI.ClientID %>").text('');
                            // $("#<%= lblAssistantSurgeonMedicaidID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                            $("#<%= lblAssistantSurgeonFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                            $("#<%= lblAssistantSurgeonLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                            $("#<%= hdnAssistSurMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                            $("#<%= hdnAssistFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                            $("#<%= hdnAssistLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
                            var medid = result[0]["MEDICAID_ID"];
                            if (medid !== null && medid !== undefined) {
                                $("#<%= errAssistantSurgeonNPI.ClientID %>").html("");
                                var APIToken = $("[id*=hdnAccessToken]").val();
                                $.ajax({
                                    type: "GET",
                                    url: webApiClaims + "GetAsstnpiCount?AsstNpi=" + txtrefNpiCode + "&&AsstMedId=" + medid + "&&ClaimType=" + hdnAsstProviderClaimType + "", 
                                    headers: {
                                        "Access-Control-Allow-Origin": "*",
                                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                        "Authorization": "Bearer " + APIToken
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        if (result === "Non-individual provider cannot be entered as referring provider") {
                                            if (hdnAsstProviderClaimType == "0" || hdnAsstProviderClaimType == "2") {
                                                $("#<%= errAssistantSurgeonNPI.ClientID %>").html("Non - individual provider cannot be entered as assistant surgeon");
                                                AsstClerFields();
                                                return false;
                                            }
                                            if (hdnAsstProviderClaimType == "1") {
                                                $("#<%= errAssistantSurgeonNPI.ClientID %>").html("Non - individual provider cannot be entered as other operative physician");
                                                AsstClerFields();
                                                return false;
                                            }
                                        }
                                        if (result === "Non - individual provider cannot be entered as other operative physician") {
                                            $("#<%= errAssistantSurgeonNPI.ClientID %>").html("Non - individual provider cannot be entered as other operative physician");
                                             AsstClerFields();
                                             return false;
                                         }
                                        else {
                                            $("#<%= errAssistantSurgeonNPI.ClientID %>").text('');
                                            // $("#<%= lblAssistantSurgeonMedicaidID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                                            var AssistantSurgeonMedicaidID = $("#<%= lblAssistantSurgeonMedicaidID.ClientID %>").html();
                                            localStorage.setItem("AssistantSurgeonMedicaidID", AssistantSurgeonMedicaidID);
                                            $("#<%= lblAssistantSurgeonFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                                            $("#<%= lblAssistantSurgeonLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                                            $("#<%= hdnAssistSurMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                                            $("#<%= hdnAssistFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                                            $("#<%= hdnAssistLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
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
        else if (AsstnpiLenth == 0) {
            $("#<%= lblAssistantSurgeonMedicaidID.ClientID %>").html('');
            $("#<%= lblAssistantSurgeonFirstName.ClientID %>").html('');
            $("#<%= lblAssistantSurgeonLastName.ClientID %>").html('');

            $("#<%= lblAssistantSurgeonMedicaidID.ClientID %>").text('');
            $("#<%= lblAssistantSurgeonFirstName.ClientID %>").text('');
            $("#<%= lblAssistantSurgeonLastName.ClientID %>").text('');

            $("#<%= hdnAssistSurMedId.ClientID %>").val('');
            $("#<%= hdnAssistFirstName.ClientID %>").val('');
            $("#<%= hdnAssistLastName.ClientID %>").val('');
        }
        return false;
    }
    function AsstClerFields() {
        localStorage.setItem("AsstProviderNPI", "");
        localStorage.setItem("AssistantSurgeonMedicaidID", "");
        $("#<%= txtAssistantSurgeonNPI.ClientID %>").val("");
        $("#<%= lblAssistantSurgeonMedicaidID.ClientID %>").text('');       
        $("#<%= lblAssistantSurgeonLastName.ClientID %>").text('');        
        $("#<%= lblAssistantSurgeonFirstName.ClientID %>").text('');

        $("#<%= hdnAssistSurMedId.ClientID %>").val('');
        $("#<%= hdnAssistFirstName.ClientID %>").val('');
        $("#<%= hdnAssistLastName.ClientID %>").val('');

    }
</script>
<asp:UpdatePanel ID="upnlAssistantSurgeon" runat="server">
    <ContentTemplate>
        <div class="row" style="background-color: #ADD8E6;">
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 140px;"><span style="color:red">*</span>NPI</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 70px;">Medicaid ID</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 30px;">Last Name</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">First Name</span>
            </div>
        </div>
        <div class="row" style="padding-left: 100px;">
            <div class="col-sm-3 ">
                <div class="row" id="lblAssistantSurgeonNPI" runat="server">
                   
                    <div class="col-sm-12">
                        <span style="text-align: left;">
                            
                            <asp:TextBox OnChange="return loaderAssistantSurgeonDental(this);" ID="txtAssistantSurgeonNPI" runat="server" CssClass="formFieldTextBox"  MaxLength="10" Style="height: 30px; width: 200px;"></asp:TextBox>
                           <asp:Label id="SearchAss" runat="server">
                               <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
        
                            <button type="button"  id="btnAssistantSurgeon" onclick="getbuttondetail(this)" class="btn btn-link" data-toggle="modal" data-target="#myModal">
                                Search
                            </button>
                            <br />
            
                             <asp:RequiredFieldValidator ID="rfvAssistantSurgeonNPI" runat="server" ControlToValidate="txtAssistantSurgeonNPI"
                                ErrorMessage="<div id='msg8_txtAssistantSurgeonNPI' ><a href='#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeonPanel_txtAssistantSurgeonNPI' class='errorlist'> *Assistant Surgeon  NPI 10-digit number is required.</a></div>" Display="None" ValidationGroup="validate11" CssClass="failureNotification"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="revRefNPI"
                               Display="None" CssClass="failureNotification"
                                    ValidationGroup="validate"
                                    ControlToValidate="txtAssistantSurgeonNPI"
                                    ValidationExpression="^[0-9]{10}$"
                                    Text =""
                                    ErrorMessage="">
                            </asp:RegularExpressionValidator>
         
                               </asp:Label>

                        </span>
                        <asp:Label ID="errAssistantSurgeonNPI" Style="color: red" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="AssistantSurgeonMedicaidID" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: justify;">
                            <asp:Label ID="lblAssistantSurgeonMedicaidID" runat="server" Style="font-size: 20px; padding-left: 40px;" Text="" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="AssistantSurgeonLastName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: justify;">
                            <asp:Label ID="lblAssistantSurgeonLastName" runat="server" Style="font-size: 20px; padding-left: 50px;" Text="" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="AssistantSurgeonFirstName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: justify;">
                            <asp:Label ID="lblAssistantSurgeonFirstName" runat="server" Style="font-size: 20px; padding-left: 70px;" Text="" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnAssistantSurgeonClaimID" runat="server" />
        <asp:HiddenField ID="hdnAssistantSurgeonClaimType" runat="server" />
        <asp:HiddenField ID="hdnErrorMessageAssistant" runat="server" />
         <asp:HiddenField ID="hdnAssistSurMedId" runat="server" />
         <asp:HiddenField ID="hdnAssistFirstName" runat="server" />
         <asp:HiddenField ID="hdnAssistLastName" runat="server" />
        <div id="divAssistantSurgeonErrorMessage" class="row" runat="server">
    <div class="col-sm-12">
    <asp:Label ID="lblAssistantSurgeonError" runat="server" ForeColor="Red" class="col-sm-12"></asp:Label>
</div>
</div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Panel runat="server" ID="pnlsepAssistanSurgeonSearch" class="CollapsingSeparator" Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerOtherProviderInfoSearch">
    <span id="sepAssistanSurgeonSearch" runat="server" class="pageHeader pH2">Search</span>
</asp:Panel>
<asp:Panel ID="pnlAssistanSurgeonSearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

    <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
        <div class="row" style="text-align: center;">
            <div class="col-sm-6 col-md-4 col-lg-3 ">
                <span class="ohio-field-label"><span style="color: red"></span><b>NPI </b>
                    <asp:TextBox ID="TextBox15" runat="server" CssClass="formField" MaxLength="10" />
                    <asp:LinkButton ID="LinkButton3" Text="Search" runat="server" ToolTip="Search"
                        CommandName="OtherProviderInfoSearch"
                        OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                </span>

            </div>

            <div class="col-sm-6 col-md-4 col-lg-3 ">
                <span class="ohio-field-label"><b>Medicaid ID </b>
                    <asp:TextBox ID="TextBox16" runat="server" CssClass="formField" MaxLength="7" />
                </span>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 ">
                <span class="ohio-field-label"><b>Business/Last Name </b>
                    <asp:TextBox ID="TextBox17" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="70" />
                </span>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 ">
                <span class="ohio-field-label"><b>First Name </b>
                    <asp:TextBox ID="TextBox18" runat="server" CssClass="formField" MaxLength="35" />
                </span>
            </div>

            <div class="divAssistantSurgeonSearch" style="padding-top: 10px">
                <asp:GridView ID="GridView2" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
                    EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-right: 100px">

                    <Columns>
                        <asp:BoundField DataField="NPI" HeaderText="NPI" />
                        <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" />
                        <asp:BoundField DataField="BusinessLastName" HeaderText="Business/Last Name" />
                        <asp:BoundField DataField="FirstName" HeaderText="First Name" />

                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
        </div>


    </div>

</asp:Panel>
