<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CPCAttestation, App_Web_glma3lal" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>
<%@ register src="~/UserControls/Address.ascx" tagname="Address" tagprefix="uc" %>
<script src="../Scripts/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript">
    function GetSelectedItem() {
        debugger;
        var CHK = document.getElementById("<%=chbxlAttestation.ClientID%>");
        var checkbox = CHK.getElementsByTagName("input");
        var listOfSpans = CHK.getElementsByTagName('span');

        let cbxselection = "";

        for (var i = 0; i < checkbox.length; i++) {
            if (checkbox[i].checked) {
                cbxselection += listOfSpans[i].attributes["ID"].value + "=true|";
            }
            else {
                cbxselection += listOfSpans[i].attributes["ID"].value + "=false|";
            }
        }

        //alert(cbxselection);
        document.getElementById("<%=hidSelection.ClientID%>").value = cbxselection;
        //$("#hidSelection").val(cbxselection);
        return true;
    }

</script>
<div>
    <asp:validationsummary id="vsCPCAcknowledgement" runat="server" displaymode="List" validationgroup="CPCAcknowledgement" CssClass="failureNotification" />
</div>

<div class="popTitle">
    <asp:label id="lblTitle" cssclass="bodyTextBold groupMemberTitle" runat="server" text="CPC Provider Attestation Aggrements" />
</div>
<asp:updatepanel id="upProv" runat="server" updatemode="Conditional">

	<ContentTemplate>
		<div id="PrimaryContactAddress" runat="server" style="width: 100%;">
			<div id="ParentTable" runat="server">
				
                <div class="row">
					<div class="col-sm-12  text-left" style="text-align: left; margin: auto;">
						<asp:CheckBoxList runat="server" ID="chbxlAttestation" CausesValidation="true">


						</asp:CheckBoxList>
					</div>
				</div>

                <%--<div class="row">
                    <div class="col-sm-3  text-right">
                    </div>
                    <div class="col-sm-9">
						<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" ValidationGroup="CPCAcknowledgement"
                            ToolTip="Save current screen data"/>
                    </div>
                </div>--%>


            </div>
        </div>
    </ContentTemplate>
</asp:updatepanel>
<asp:textbox id="hidIsEdit" runat="server" visible="false" />
<asp:textbox id="hidID" runat="server" visible="false" />
<asp:textbox ID="hidSelection" runat="server" text="" style="display:none;" /> <%--ClientIDMode="Static"   --%>

