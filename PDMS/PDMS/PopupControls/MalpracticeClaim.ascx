<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_MalpracticeClaim" Codebehind="MalpracticeClaim.ascx.cs" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>


<style type="text/css">
    .auto-style2 {
        height: 23px;
    }
    .auto-style5 {
        height: 20px;
    }
    .custom-select{
        min-width: 240px;
        height: 34px;
         border: 1px solid #ccc;
    }

</style>
<script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        var rbl = $("#<%= rblMalpractice.ClientID %> input:checked").val();

        if (rbl == "1")
            showAddMalpracticeClaimBtn(rbl);
    });

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
    }
    function IsNumeric(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    function showAddMalpracticeClaimBtn(selectedValue) {
        var divmal = document.getElementById("<%=ParentTable.ClientID%>");

        if (selectedValue == "1") {
            $("#ctl00_MainContent_ucMalpracticeClaim_ParentTable").show();
            divmal.style.display = "block";
        }
        else {
            divmal.style.display = "none";
           
            $("#ctl00_MainContent_ucMalpracticeClaim_ParentTable").hide();           

        }
    }
</script>

<asp:Panel runat="server" ID="pnlMalpracticeClaim" Style="padding-left: 6px;" Visible="false">
    <br />
    Have you had any professional liability actions (pending, settled, arbitrated, mediated or litigated) within the past 10 years?&nbsp;
    <table role="presentation">
        <tr>
        <td style="align-content: center;">
            <asp:RadioButtonList ID="rblMalpractice" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList" OnSelectedIndexChanged="rblMalpractice_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>

            </asp:RadioButtonList>
        </td>
        </tr>
    </table>
   
</asp:Panel>

<div class="divGrid" style="padding-top: 10px">
    <asp:GridView runat="server" Width="99.5%" ID="grdMalpracticeClaim" AutoGenerateColumns="False" HorizontalAlign="Left" Caption="Malpracriceclaims"
        CssClass="gridview" EmptyDataText="No MalpracticeClaim found." OnRowCommand="grd_RowCommand" DataKeyNames="REG_MALPRACTICE_CLAIM_ID">
        <Columns>
            <asp:BoundField DataField="DateOccurance" HeaderText="Date of Occurrence" DataFormatString="{0:M/d/yyyy}" />
                
            <asp:BoundField DataField="Claim_Status_Name" HeaderText="Status Of Claim" />
            <asp:BoundField DataField="DateCLAIMFiled" HeaderText="Date Claim Filed" DataFormatString="{0:M/d/yyyy}" />
            <asp:BoundField DataField="ProfessionalCarrier" HeaderText="Professional liability carrier involved" />
            <asp:BoundField DataField="SETTLEMENT_AMOUNT" HeaderText="Amount of the settlement" />
            <asp:BoundField DataField="Resolution_Desc" HeaderText="Method of Resolution" />
            <asp:TemplateField HeaderText="Date Claim Settled">
                    <ItemTemplate>
                        <asp:Label ID="lblClaimSettleDate" runat="server" Text='<%#Helper.GetDisplayFormatDate(Eval("CLAIM_SETTLED_DATE")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRegAddressId" runat="server" Value='<%# Eval("REG_ADDRESS_ID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Headertext="Edit">
                <ItemTemplate>
                    <asp:ImageButton ID="btnEditMalpracticeClaim" AlternateText="edit" runat="server" CommandName="MalpracticeClaim" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                        ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnMalpracticeClaims" runat="server" ImageUrl="~/Images/add.png" AlternateText="Add new" OnCommand="lbtnAdd_Click" CommandName="AddMalpracticeClaim" ToolTip="Add" Visible="true" /><br />
    </div>
</div>

<div id="malpracticeDetail" runat="server" visible="false">
    <div>
        <asp:ValidationSummary ID="vsMalpracticeClaimInfo" runat="server" DisplayMode="List" ValidationGroup="valMalpracticeClaimInfo"  CssClass="failureNotification"  />

    </div>
    <br />
    <div id="ParentTable" runat="server">
        <div class="row" id="trDateOccurence">
            <div class="col-sm-3 text-right"><span class="formLabel200">Date of Occurence*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtDateOccurence" aria-label="DateOccurence" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtDateOccurence" runat="server" />

                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valMalpracticeClaimInfo"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOccurence"
                    ErrorMessage="Select a valid Date of Occurence" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
                <%--<asp:RequiredFieldValidator ID="rfDateOccurence" runat="server" ControlToValidate="txtDateOccurence" ErrorMessage="Date Of Occurence is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>
        <div class="row" id="trDateClaimFiled">
            <div class="col-sm-3 text-right"><span class="formLabel200">Date Claim Filed*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtDateClaimFiled" aria-label="DateClaimFiled" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtDateClaimFiled" runat="server" />

                <asp:CompareValidator ID="CompareValidator3" runat="server" ValidationGroup="valMalpracticeClaimInfo"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateClaimFiled"
                    ErrorMessage="Select a valid Date Claim Filed" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
                <%--<asp:RequiredFieldValidator ID="rfDateClaimFiled" runat="server" ControlToValidate="txtDateClaimFiled" ErrorMessage="Date Claim Filed is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>
        <div class="row" id="trClaimStatus">
            <div class="col-sm-3 text-right"><span class="formLabel200">Status of the claim*</span></div>
            <div class="col-sm-9">
                <%--<asp:RadioButtonList ID="rblClaimStatus" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" CssClass="QstRadioList">
                </asp:RadioButtonList> --%>
                <asp:DropDownList ID="ddlClaimStatus" runat="server" aria-label="ClaimStatus" RepeatDirection="Horizontal" RepeatColumns="4" CssClass="formDropDown" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimStatus_SelectedIndexChanged">
              
                </asp:DropDownList>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlClaimStatus" ErrorMessage="Claim Status is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>

          <div class="row" id="trIfYesDateFiled">
            <div class="col-sm-3 text-right"><span class="wd550 fieldLabel">If settled, the date the claim was settled</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtDateSettled" aria-label="DateSettled" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtDateSettled" runat="server" />

                <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="valMalpracticeClaimInfo"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateSettled"
                    ErrorMessage="Select a valid Date Settled" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
        </div>
        <div class="row" id="trProfessionalLiability">
            <div class="col-sm-3 text-right"><span class="wd550 fieldLabel">Professional liability carrier involved*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtProfessionalLiability" aria-label="ProfessionalLiability" runat="server" CssClass="formField" MaxLength="100" onKeyUp="javascript:alphanumericOnly(this);" />
                <%--<asp:RequiredFieldValidator ID="rfProfessionalLiability" runat="server" ControlToValidate="txtProfessionalLiability" ErrorMessage="Professional Liability is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>
        <div class="row" id="trAddress">
           <span ><uc:Address id="ucAddress" runat="server" ValidationGroup="vgWorkHistoryDetails"></uc:Address></span>
        </div>
        <div class="row" id="trPolicyNo">
            <div class="col-sm-3  text-right"><span class="formLabel200">Policy Number</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtPolicyNumber" aria-label="PolicyNumber" runat="server" CssClass="formField" MaxLength="100" onKeyUp="javascript:alphanumericOnly(this);"/>                
            </div>
        </div>

          <div class="row" id="trMethodofResolution">
            <div class="col-sm-3  text-right">
                 <asp:Label ID="lblmethodResolution" runat="server" Text="Method of Resolution" CssClass="formLabel200"/>
            </div>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlMethodofResoultion" aria-label="MethodofResoultion" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" CssClass="formDropDown">                    
                </asp:DropDownList>
<%--                <asp:RequiredFieldValidator ID="rfvMethodofResolution" runat="server" ControlToValidate="ddlMethodofResoultion" ErrorMessage="Method of resolution is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>

        <div class="row" id="trSettledAmount">
            <div class="col-sm-3  text-right"><span class="wd500 fieldLabel">If settled, the amount of settlement</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtSettledAmount" aria-label="SettledAmount"   runat="server" CssClass="formField" MaxLength="25" onkeypress="return IsNumeric(event);"/>                
            </div>
        </div>
        
      

        <div class="row">
            <div class="col-sm-3  text-right"><span class="wd500 fieldLabel">Describe the allegations against you*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtAllegations" aria-label="Allegations" runat="server" CssClass="formField" TextMode="Multiline" MaxLength="150" onKeyUp="javascript:alphanumericOnly(this);"/>
                <%--<asp:RequiredFieldValidator ID="rfAllegations" runat="server" ControlToValidate="txtAllegations" ErrorMessage="Allegations is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right"><span class="wd150 formLabel300">Were You*</span></div>
            <div class="col-sm-9">
                <asp:RadioButtonList ID="rblDefendant" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                </asp:RadioButtonList>
                <%--<asp:RequiredFieldValidator ID="rfDefendant" runat="server" ControlToValidate="rblDefendant" ErrorMessage="Select Defendant Type." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>
          <div class="row">
            <div class="col-sm-3  text-right"><span class="wd350 fieldLabel">No of Other Defendants (if any)</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtnoofOtherDefendents" aria-label="noofOtherDefendents" runat="server" CssClass="formField" MaxLength="2" onkeypress="return IsNumeric(event);"/>
            </div>
        </div>
        
        
      <div class="row">
            <div class="col-sm-3  text-right"><span class="wd400 fieldLabel">Your role in case*</span></div><div class="col-sm-9">
                <asp:TextBox ID="txtRoleinCase" aria-label="RoleinCase" runat="server" CssClass="formField" MaxLength="25" onKeyUp="javascript:alphanumericOnly(this);" />
                <%--<asp:RequiredFieldValidator ID="rfvRoleinCase" runat="server" ControlToValidate="txtRoleinCase" ErrorMessage="Enter your role in case." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>--%>
            </div>
        </div>
        
        <div class="row">
            <div class="col-sm-3  text-right"><span class="wd550 fieldLabel">Describe the alleged injury to the patient</span></div>
            <div class="col-sm-9"> 
                <asp:TextBox ID="txtAllegedInjury" aria-label="AllegedInjury" runat="server" CssClass="formField" TextMode="Multiline" MaxLength="150" onKeyUp="javascript:alphanumericOnly(this);"/>                

            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right"><span class="wd550 fieldLabel">Did the alleged injury result in death?</span></div>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlIsDead" runat="server" RepeatDirection="Horizontal" aria-label="is dead" CssClass="formDropDown">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:DropDownList> 
            
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right">
                <span class="wd550 fieldLabel">To the best of your knowledge, is the case included in the NPDB?*</span>
            </div>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlIsNPDB" aria-label="IsNPDB" runat="server" RepeatDirection="Horizontal" CssClass="custom-select">
                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:DropDownList> 
            
            </div>
        </div>
       <%-- <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">Claimant/Plaintiff filed suit in court?</span></div>
            <div class="col-sm-8">
                <asp:RadioButtonList ID="rblClaimantOrPlaintiff" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:RadioButtonList> <asp:RequiredFieldValidator ID="rflClaimantOrPlaintiff" runat="server" ControlToValidate="rblClaimantOrPlaintiff" ErrorMessage="Select Claimant Or Plaintiff is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>
            
            </div>
        </div>
      
        <div class="row" id="trCaseNumber">
            <div class="col-sm-3 text-right"><span class="formLabel200">State Court Case Number</span></div>
            <div class="col-sm-8">
                <asp:TextBox ID="nbCaseNumber" runat="server" MaxLength="100" CssClass="formField" />

                <asp:RequiredFieldValidator ID="rfCaseNumber" runat="server" ControlToValidate="nbCaseNumber" ErrorMessage="Case Number is required" Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="false"></asp:RequiredFieldValidator>

            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">State</span></div>
            <div class="col-sm-8">
                <asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown" AutoPostBack="true" AppendDataBoundItems="true"  OnSelectedIndexChanged="ddlState_SelectedIndexChanged"/>
                <asp:RequiredFieldValidator ID="rfState" runat="server" ControlToValidate="ddlState" ErrorMessage="State is required" Text="*" 
                    ValidationGroup="valMalpracticeClaimInfo" Enabled="false" Display="Dynamic" InitialValue="" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">County</span></div>
            <div class="col-sm-8">
                <asp:DropDownList ID="ddlCounty" runat="server" CssClass="formDropDown" />
            </div>
        </div>
        <div class="row" id="trFederalCaseNumber">
            <div class="col-sm-3 text-right"><span class="formLabel300 wd350">Federal Court(U.S. District Court) Case Number</span></div>
            <div class="col-sm-8">
                <asp:TextBox ID="nbFederalCaseNumber" runat="server" MaxLength="9" CssClass="formField" />
                <asp:RequiredFieldValidator ID="rfFederalCaseNumber" runat="server" ControlToValidate="nbFederalCaseNumber" ErrorMessage="Federal Court(U.S. District Court) Case Number is required" Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="false"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">District</span></div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtDistrict" runat="server" CssClass="formField" />
                <asp:RequiredFieldValidator ID="rfFederalDistrict" runat="server" ControlToValidate="txtDistrict" ErrorMessage="District is required" Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="false"></asp:RequiredFieldValidator>

            </div>
        </div>
        
        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">If Other, enter status here</span></div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtOtherStatus" runat="server" CssClass="formField" />
                <asp:RequiredFieldValidator ID="rfvOtherStatus" runat="server" ControlToValidate="txtOtherStatus" ErrorMessage="Other Status is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="false"></asp:RequiredFieldValidator>

            </div>
        </div>
        <div class="row">
            Please provide additional information/explanation (e.g. the condition/diagnosis of the patient at the time of the incident, treatment rendered and the condition of the patient subsequent to treatment)
        </div>
        <div class="row">
                <asp:TextBox ID="txtinfo" runat="server" TextMode="MultiLine" CssClass="formField wdAll" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtinfo" ErrorMessage="Additional Information is required." Text="*" ValidationGroup="valMalpracticeClaimInfo" Enabled="true"></asp:RequiredFieldValidator>                    
        </div>--%>
    </div>
    <br />
    <asp:HiddenField ID="hdnMalPracticeId" runat="server" />
</div>
<asp:HiddenField ID="hidID" runat="server" Visible="false" />
