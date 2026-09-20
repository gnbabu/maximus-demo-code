<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_DMEProductsAndServices, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
<script type="text/javascript">
    var specialKeys = new Array();
     specialKeys.push(8);  //Backspace
     specialKeys.push(9);  //Tab
     specialKeys.push(46); //Delete
     specialKeys.push(36); //Home
     specialKeys.push(35); //End
     specialKeys.push(37); //Left
     specialKeys.push(39); //Right
 
     function IsAlphaNumeric(e) {
         var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
         var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || keyCode == 32 || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
         return ret;
     }
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
    function alphabetsOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z]/g, '');
    }
</script>


<style type="text/css">
    .table {
        margin-right:0px !important;
        margin-left:0px !important;
    }
</style>
<asp:UpdatePanel ID="upProd" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress runat="server" ID="upProgress" DisplayAfter="0">
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />Loading...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <div style="width: 100%">
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:ValidationSummary ID="vsDMEProductsAndServices" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valDMEProductsAndServices" />
        </div>
        <div id="AddressTable">
            <div class="row">
                <div class="col-sm-5 text-right">
                    <asp:Label ID="lblCategory" runat="server" Text="Product/Service Category*" CssClass="formLabel200"></asp:Label></div>
                <div class="col-sm-7 text-left">
                    <asp:DropDownList ID="ddlProductsServices" runat="server" MaxLength="35" CssClass="formField" OnSelectedIndexChanged="ddlProductsServices_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProctsServices" runat="server" ControlToValidate="ddlProductsServices" Enabled="true" SetFocusOnError="true"
                        Display="Dynamic" Text="*" ValidationGroup="valDMEProductsAndServices" ErrorMessage="* Products And Services is required."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5 text-right">
                    <asp:Label ID="lblSubCategory" runat="server" Text="Product/Service Sub-Category*" Visible="false" CssClass="formLabel200"></asp:Label></div>
                <div class="col-sm-7 text-left">
                    <asp:CheckBoxList ID="chkSubCategory" runat="server" MaxLength="35" CssClass="formCheckBoxDME" Visible="false"></asp:CheckBoxList>

                </div>

            </div>
            <div class="row">
                <div class="col-sm-5 text-center">
                    <asp:Label ID="lblQualifications" runat="server" Text="Qualifications to Provide Specific Products*" Visible="false" CssClass="formLabel200"></asp:Label></div>
                <div class="col-sm-7" style="margin-left:0px !important;">
                    <asp:CheckBoxList ID="chkQualifications" runat="server" MaxLength="35" CssClass="formCheckBoxDME" Visible="false"></asp:CheckBoxList>                    
                </div>                
            </div>
            <div class="row">
                <p class="pg-hint2">
                        <asp:Label ID="lblQualificationscomment" runat="server" Visible="false" Text="*Please Enter any associated licenses, certifications and trainings that qualifies you to furnish this product"></asp:Label>                        
                    </p>
            </div>
            <br />
            <div class="row">
                <div class="col-sm-5 text-right"><span class="formLabel wd200"></span></div>
                <div class="col-sm-7 text-left">
                    <asp:TextBox ID="txtQualifications" runat="server" TextMode="MultiLine" Visible="false" CssClass="formField" onkeypress="return IsAlphaNumeric(event);" ondrop="return false;" onpaste="return false;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtQualifications" runat="server" ControlToValidate="txtQualifications" Enabled="false" SetFocusOnError="true"
                        Display="Dynamic" Text="*" ValidationGroup="valDMEProductsAndServices" ErrorMessage="* Qualifications to Provide Specific Products is required."></asp:RequiredFieldValidator></div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>









