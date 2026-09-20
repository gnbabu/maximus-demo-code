<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_DentalLicense" Codebehind="DentalLicense.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
 <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<style type="text/css">
    .helpText{
        padding: 5px;
        display: none;
        margin-top: 5px;
        border: 0px solid #000;
    }
    .red{ background: #ff0000; }
    .green{ background: #00ff00; }
    .blue{ background: #0000ff; }
</style>
<script type="text/javascript">


    function getvalueofradiolist() {


        if (($('#<%=rblNO.ClientID %> input[type=radio]:checked').val() == 'True') || ($('#<%=rblsedation.ClientID %> input[type=radio]:checked').val() == 'True') || ($('#<%=rblanesthesia.ClientID %> input[type=radio]:checked').val() == 'True')) {
            
            $(".dentalLic").show();
        }
        else {  
             
            $(".dentalLic").hide();
        }
    
}

function ValidateModuleList(source, args) {
    var chkListModules = document.getElementById('<%= cklLicenseStatus.ClientID %>');
    var chkListinputs = chkListModules.getElementsByTagName("input");
    for (var i = 0; i < chkListinputs.length; i++) {
        if (chkListinputs[i].checked) {
            args.IsValid = true;
            return;
        }
    }
    args.IsValid = false;
}


function ValidateTextBox(source, args) {
    var chkListModules = document.getElementById('<%= cklLicenseStatus.ClientID %>');
    var chkListinputs = chkListModules.getElementsByTagName("input");
    var txtOther = document.getElementById('<%= txtOther.ClientID %>');
    
    if (chkListinputs[4].checked && txtOther.value == "") {
        args.IsValid = false;
            return;
        }
        args.IsValid = true;
    }

    function ValidateSpecilaistTextBox(source, args) {
        var rbListModules = document.getElementById('<%= rblSpecialist.ClientID %>');
        var txtSpecialist = document.getElementById('<%= txtSpecialistYN.ClientID %>');
        var rbListinputs = rbListModules.getElementsByTagName("input");
                for (var i = 0; i < rbListinputs.length; i++) {
                    if (rbListinputs[0].checked) {
                        if (rbListinputs[i].val = "True" && txtSpecialist.value == "") {
                            args.IsValid = false;
                            return;
                        }
                        else {
                            args.IsValid = true;
                        }
                    }
                }
                args.IsValid = true;
            }
</script>

<asp:Panel ID="pnlDentalLicenses" runat="server" Style="display: inline-block; width: 100%;">
    <p style="text-align: center; font-weight: bold">A copy of each license must be uploaded to this page.</p>
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdDentalLicense" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No licenses found" OnRowCommand="grd_RowCommand" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="LICENSE_TYPE_DETAILS" HeaderText="License Status" SortExpression="LICENSE_TYPE_DETAILS" HtmlEncode="False" />
                <asp:BoundField DataField="SPECIALIST" HeaderText="Recognized as Specialist" SortExpression="SPECIALIST" />
                <asp:BoundField DataField="ANESTHESIA_PERMIT" HeaderText="Administer Anesthesia" SortExpression="ANESTHESIA_PERMIT" />
                <asp:BoundField DataField="SEDATION_PERMIT" HeaderText="Administer Conscious Sedation" SortExpression="SEDATION_PERMIT" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit"  alt="EditButton" runat="server" CommandName="EditDentalLicensesCodeRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                     <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteDentalLicensesCodeRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                           ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete" 
                           Visible="<%# CanUserViewDelete()  %>"/>
                    </ItemTemplate>
              </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddDentalLicenses" runat="server" ImageUrl="~/Images/add.png" CommandName="DentalLicenses" OnCommand="lbtnAdd_Click" ToolTip="Add" />
        <asp:ImageButton ID="btnDentalLicensesHistory" CommandName="DentalLicenses" runat="server" ImageUrl="~/Images/history_icon.jpg"
            OnCommand="btnHistory_Click" ToolTip="History" />
    </div>
    <br />
</asp:Panel>

<div id="dentalDetail" runat="server" visible="false">
    <div  style="text-align:left;margin:10px;" >
    <asp:ValidationSummary ID="vsDentalLicense" runat="server" DisplayMode="List" CssClass="failureNotification" ValidationGroup="valDentalLicense" />
    
</div>
  
 
  <div  id="ParentTable" runat="server" style="width:auto;margin-left:50px;">

    
        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">Licensure status (please check all that apply)</span></div>
            <div class="col-sm-9 text-left">
                <asp:CheckBoxList runat="server" ID="cklLicenseStatus" CausesValidation="true" >
                      <asp:ListItem Text="General dental license" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Limited dental license" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Teacher’s dental license" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Inactive dental license" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="5"></asp:ListItem>
                </asp:CheckBoxList>
            
        <asp:CustomValidator runat="server" ID="cvlLicenseStatus"
      ClientValidationFunction="ValidateModuleList"
      ErrorMessage="Please Select a license type" ValidationGroup="valDentalLicense"></asp:CustomValidator>

        <%--        <asp:CustomValidator ID="cvLicenseStatus" runat="server" ErrorMessage="CustomValidator" ControlToValidate="" OnServerValidate="cvLicenseStatus_ServerValidate" ValidationGroup="valDentalLicense" ></asp:CustomValidator>
       --%>          </div>
   
        </div>
        <div class="row" id="trOther">
             <div class="col-sm-3 text-right"><span class="formLabel200">If 'Other', Explain</span></div>
            <div class="col-sm-9 text-left">
                <asp:TextBox ID="txtOther" runat="server" CssClass="formField" TextMode="MultiLine" />  
                   <asp:CustomValidator runat="server" ID="cvtxtOther" ValidateEmptyText="true"
      ClientValidationFunction="ValidateTextBox"
      ErrorMessage="Please explain other license type" ValidationGroup="valDentalLicense" Text="*"></asp:CustomValidator>    
         <%--        <asp:RequiredFieldValidator runat="server" ID="rfvOther"  ControlToValidate="txtOther" ErrorMessage="* Please Explain" SetFocusOnError="true" Enabled="true"></asp:RequiredFieldValidator>
    --%>
       
            </div>
        </div>

        <div class="row" id="trSpecialist">
            <div class="col-sm-3 text-right"><span class="formLabel200">Are you recognized as a specialist by the Dental Board?</span></div>
            <div class="col-sm-9 text-left">
                <asp:RadioButtonList ID="rblSpecialist" runat="server" RepeatDirection="Horizontal" >
                      <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:RadioButtonList>
                
            </div>
        </div>

        <div class="row" id="tr1">
            <div class="col-sm-3 text-right"><span class="formLabel200">If 'Yes', Explain</span></div>
            <div class="col-sm-9 text-left">
                <asp:TextBox ID="txtSpecialistYN" runat="server" CssClass="formField" TextMode="MultiLine" />  
                    <asp:CustomValidator runat="server" ID="cvtxtSpecialistYN" ValidateEmptyText="true"
      ClientValidationFunction="ValidateSpecilaistTextBox"
      ErrorMessage="Please explain" ValidationGroup="valDentalLicense" Text="*" ></asp:CustomValidator>
             </div>
        </div>

           <div class="row" id="tr2">
            <div class="col-sm-3 text-right"><span class="formLabel200">Do you hold a permit to administer a general anesthesia?</span></div>
            <div class="col-sm-9 text-left">
                <asp:RadioButtonList ID="rblanesthesia" runat="server" RepeatDirection="Horizontal"  onclick="getvalueofradiolist();"  >
                      <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:RadioButtonList>
             
                    
            </div>
        </div>

           <div class="row" id="tr3">
            <div class="col-sm-3 text-right"><span class="formLabel200">Do you hold a permit to administer conscious sedation?</span></div>
            <div class="col-sm-9 text-left">
                <asp:RadioButtonList ID="rblsedation" runat="server" RepeatDirection="Horizontal" onclick="getvalueofradiolist();" >
                      <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:RadioButtonList>
            
            </div>
        </div>

           <div class="row" id="tr4">
            <div class="col-sm-3 text-right"><span class="formLabel200">Do you utilize nitrous oxide in your practice?</span></div>
            <div class="col-sm-9 text-left">
                <asp:RadioButtonList ID="rblNO" runat="server" RepeatDirection="Horizontal" onclick="getvalueofradiolist();"  >
                      <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                </asp:RadioButtonList>
 
            
            </div>
               </div>
            <div class="row">
                <%--<div class="col-sm-9 text-left"></div>--%>
                <div class="col-sm-9 text-left"   style="height:30px; vertical-align:top;">
                     <div   class="dentalLic infoBox"  >
     
        <div class="infoContent">
           Plesse upload a copy of DEA license.
        </div>
    </div>  
                </div>
            </div>
        </div>

<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
</div>
