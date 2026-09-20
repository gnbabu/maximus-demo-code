<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceEnrollmentAndDisenrollment" Codebehind="HospiceEnrollmentAndDisenrollment.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#txtElectionDate").blur(function () {
            $("#hdnElectionDate").val($("#txtElectionDate").val());
        });
        $("#txtDisenrollment").blur(function () {
            $("#hdnDisenrollment").val($("#txtDisenrollment").val());
        });
    });

</script>
<ajax:Accordion ID="Accordion1" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">
    <Panes>
        <ajax:AccordionPane ID="AccordionPane1" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblEnrollmentAndDisEnrollmentExpansion" class="expandcollapse" runat="server" Text="- * ENROLLMENT AND DISENROLLMENT"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="width: 97%; padding: 10px;">
                    <div class="col-md-2" style="text-align: right;">
                        <span class="ohio-field-label"><span style="color: red">*</span> <b>Election Date:</b>  </span>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtElectionDate" Width="80%" autocomplete="off" ClientIDMode="static"   runat="server">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdnElectionDate" runat="server" />
                        <ajax:CalendarExtender ID="clElectionDate" TargetControlID="txtElectionDate" runat="server" Format="MM/dd/yyyy" />
                        <asp:RequiredFieldValidator ID="rfvtxtElectionDate" runat="server" ControlToValidate="txtElectionDate" isplay="Dynamic" ForeColor="Red" ErrorMessage="Please select election date" SetFocusOnError="True" ValidationGroup="vgOnSubmit"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-3" style="text-align: right;">
                        <span class="ohio-field-label"><span style="color: red">*</span> <b>Disenrollment Date:</b>
                        </span>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtDisenrollment" Width="80%" autocomplete="off"   runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hdnDisenrollment" runat="server" />
                        <ajax:CalendarExtender ID="ClDisenrollment" TargetControlID="txtDisenrollment" runat="server" Format="MM/dd/yyyy" />
                      <%--  <asp:RequiredFieldValidator ID="rfvtxtDisenrollment" runat="server" ControlToValidate="txtDisenrollment" ErrorMessage="Please select disenrollment date"
                            Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="vgOnSubmit"></asp:RequiredFieldValidator>--%>
                    </div>
                    <div class="col-md-1">
                    </div>
                </div>
                 
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>



