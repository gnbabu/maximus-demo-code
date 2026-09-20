<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OrientationInfo" Codebehind="OrientationInfo.ascx.cs" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupOrgEventHandlers);
    });

    function setupOrgEventHandlers() {
        $("#divRequestedEffectiveDateInfo").hide();

        if ($("#ctl00_MainContent_ucIdentification_ucOrgInfo_rblCitizenAlien input").length) {
            if ($("#ctl00_MainContent_ucIdentification_ucOrgInfo_rblCitizenAlien_0")[0].checked == true) {
                $("#tblImmigration").hide();
            }
            else {
                $("#tblImmigration").show();
            }
        }

        $(".what-is-this-link").mouseover(function () {
            $("#divRequestedEffectiveDateInfo").show();
        });

        $("#divRequestedEffectiveDateInfo").mouseleave(function () {
            $("#divRequestedEffectiveDateInfo").hide();
        });

        $(".help-kfe").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            var keyfield = $(this).text();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            $(".key-field-title").text(keyfield);
            //show the menu directly over the placeholder

            if (this.id == "helpNPI") {
                $("#helpKFEEditInfoUp").css({
                    position: "absolute",
                    top: pos.top + "px",
                    left: (pos.left + width) + "px"
                }).show();
            }
            else {
                $("#helpKFEEditInfo").css({
                    position: "absolute",
                    top: pos.top + "px",
                    left: (pos.left + width) + "px"
                }).show();
            }

        });

        $("#helpKFEEditInfo").mouseleave(function () {
            $("#helpKFEEditInfo").hide();
        });

        $("#helpKFEEditInfoUp").mouseleave(function () {
            $("#helpKFEEditInfoUp").hide();
        });

        $(".help-taxid").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpTaxIDInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });

        $("#helpTaxIDInfo").mouseleave(function () {
            $("#helpTaxIDInfo").hide();
        });


        if ($("#ctl00_MainContent_ucIdentification_ucOrgInfo_rblCitizenAlien input").length) {
            $("#ctl00_MainContent_ucIdentification_ucOrgInfo_rblCitizenAlien input").change(function (e) {
                if ($("#ctl00_MainContent_ucIdentification_ucOrgInfo_rblCitizenAlien_0")[0].checked == true) {
                    $("#tblImmigration").hide();
                }
                else {
                    $("#tblImmigration").show();
                }
            });
        }
    }


</script>

<div>
    <asp:ValidationSummary ID="vsOrientationInfo" runat="server" DisplayMode="List" ValidationGroup="valOrientationInfo" />
</div>
<asp:HiddenField ID="hdnRegOrientationID" runat="server" Value="" />
<div id="ParentTable" runat="server" style="width: auto;">
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel wd170">Organization Name</span></div>
        <div class="col-sm-9 text-left">
            <asp:Label ID="lblOrgName" runat="server" CssClass="formField" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel wd170">Due By</span></div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtDueByDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="CalendarExtender7" TargetControlID="txtDueByDate" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel wd170">Schedule Date</span></div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtScheduleDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="CalendarExtender6" TargetControlID="txtScheduleDate" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel wd170">Provider Response Date</span></div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtResponseDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="CalendarExtender8" TargetControlID="txtResponseDate" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel wd170">Status</span></div>
        <div class="col-sm-9 text-left">
            <asp:DropDownList ID="ddlOrientationStatus" runat="server" CssClass="formField" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel wd170">Comments</span></div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtComments" runat="server" CssClass="formField" />
        </div>
    </div>
</div>

