<%@ control language="C#" autoeventwireup="true" inherits="Usercontrols_AddressValidation, App_Web_addressvalidation.ascx.6bb32623" %>

<asp:HiddenField ID="hdnAddressConfirm" runat="server" Value="0" />

<div runat="server" id="divConfirmAddress" style="display:none; text-align:center">
    <p style="color:darkgreen">The address has been corrected. Your original values are listed below:</p> 
    <p style="color:darkgreen" id="paraUSPSAddress" runat="server"></p>
    <p style="color:darkgreen">Click on 'Accept Corrections' to confirm acknowledgement of this message.</p>
    <asp:Button ID="btnConfirmAddress" class="buttonBox" Text="Accept Corrections" runat="server" />
</div>

<asp:CustomValidator id="cvAddress" 
    ControlToValidate="" 
    OnServerValidate="ServerValidation" 
    Display="None" 
    ErrorMessage="" 
    ValidationGroup="valOwnerInfo"
    runat="server" />

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
</script>