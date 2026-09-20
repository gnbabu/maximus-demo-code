<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SpecialityFocus" Codebehind="SpecialityFocus.ascx.cs" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<script type="text/javascript">
   function pageLoad() {
        $('[data-toggle="popover"]').popover()
    }
</script>
<asp:Panel ID="pnlSpecialtyFocus" runat="server"> 

<div class="divGrid">
    <div class="row">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblEndorsemnetNumber" runat="server" Text="Endorsement Number" CssClass="formLabel" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbEndorsemnetNumber" runat="server" CssClass="formField" ReadOnly ="false" aria-Label="Endorsement Number" />
                 <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='ltrlInfotext4' runat='server' Text='<%$ Resources:BrandingResource , LICENSE_FOCUS_SPECIALTY_HELPTEXT %>' />" aria-hidden="true" />
                </div> 
            </div>         

    <div class="row">
            <div class="col-sm-4 text-right">
                <asp:Label ID="Label3" runat="server" Text="Endorsement Status" CssClass="formLabel" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbEndorsemnetStatus" runat="server" CssClass="formField" ReadOnly ="false" aria-Label="Endorsement Status" />
                 <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='ltrlInfotext5' runat='server' Text='<%$ Resources:BrandingResource , LICENSE_FOCUS_SPECIALTY_HELPTEXT %>' />" aria-hidden="true" />
                </div> 
            </div>         

     
    
        <div class="row">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblFocus" runat="server" Text="Endorsement Focus" CssClass="formLabel" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbFocus" runat="server" CssClass="formField" ReadOnly ="false"  aria-Label="Endorsement Focus" MaxLength ="75"/>
                <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='ltrlInfotext1' runat='server' Text='<%$ Resources:BrandingResource , LICENSE_FOCUS_SPECIALTY_HELPTEXT %>' />" aria-hidden="true" />
                </div>         

        </div>
   
      <div class="row">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblSpecialty" runat="server" Text="Endorsement Specialty" CssClass="formLabel" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbSpecialty" runat="server" CssClass="formField" aria-Label="Endorsement Specialty" MaxLength ="75" />
                <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='ltrlInfotext2' runat='server' Text='<%$ Resources:BrandingResource , LICENSE_FOCUS_SPECIALTY_HELPTEXT %>' />" aria-hidden="true" />
            </div>        
      </div>
     <div class="row">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lbCertifyingOrg" runat="server" Text="Certifying Organization" CssClass="formLabel" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbCertifyingOrg" runat="server" CssClass="formField" aria-Label="Certifying Organization" MaxLength ="75" />
                <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='ltrlInfotext3' runat='server' Text='<%$ Resources:BrandingResource , LICENSE_FOCUS_SPECIALTY_HELPTEXT %>' />" aria-hidden="true" />
            </div>            
        </div>
    <div class="row">
            <div class="col-sm-4 text-right">
                <asp:Label ID="Label1" runat="server" Text="Certificate Date" CssClass="formLabel" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbCertificationDate" runat="server" CssClass="formField" ReadOnly ="false" aria-Label="Certificate Date"  /><ajax:CalendarExtender ID="CalendarExtenderDate" TargetControlID="tbCertificationDate" runat="server"  />
    
                </div>         

        </div>
     <div class="row">
            <div class="col-sm-4 text-right">
                <asp:Label ID="Label2" runat="server" Text="Certificate Expiration" CssClass="formLabel" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbCertificationEndDate" runat="server" CssClass="formField" ReadOnly ="false" aria-Label="Certificate Expiration"  /><ajax:CalendarExtender ID="CalendarExtenderEndDate" TargetControlID="tbCertificationEndDate" runat="server"  />
               
                </div>         

        </div>

</div>

</asp:Panel>