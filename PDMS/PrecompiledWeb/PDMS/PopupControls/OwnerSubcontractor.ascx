<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OwnerSubcontractor, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<style>
.formLabel200 {
    right:-8px;
    position:absolute;
}
    </style> 
    <script  type="text/javascript">


        function CheckSSNLength(sender, args) {
            var re = /\D/g; // Remove any characters that are not numbers
            var test = args.Value.replace(re, "");
            if (test == "") return true;

            var len = test.length;
            if (len != 9)
                args.IsValid = false;
            return;
        }
    </script>
<%--<style type="text/css">
     .table>tbody>tr>th,
    .table>tbody>tr>td {
        border:none !important;
        margin-bottom:0px !important;
     }    
</style>--%>


<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <asp:ValidationSummary ID="vsOwnerSubcontractor" runat="server" DisplayMode="List" ValidationGroup="valOwnerSubcontractor" />
        </div>
        <div>
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <ProgressTemplate>
                    <div>
                        <img src="../Images/ajax-loader.gif" alt="AJAX Loader" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <%-- <table>
            <tr>
                <td>--%>
        <%--<table id="ParentTable" runat="server" class="table">
     <tr><td>--%>
        <div id="divOwnerSubcontractor">
           <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel200">Name of Subcontractor*</span></div>        
        <div class="col-sm-9">
                    <asp:DropDownList ID="ddlSubcontractor" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="ddlSubcontractor_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="valOwnerSubcontractor"
                        ControlToValidate="ddlSubcontractor" ErrorMessage="*Select an Subcontractor" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                </div>
               <div class="pdmsLabel" style="display:none;"><asp:Label ID="Label1" runat="server" /></div>
               </div>
            </div>
                
        
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlSubcontractor" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

<asp:HiddenField ID="hdnRegSubcontractorID" runat="server" />
