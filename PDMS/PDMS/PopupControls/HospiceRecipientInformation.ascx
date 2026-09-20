<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceRecipientInformation" Codebehind="HospiceRecipientInformation.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<ajax:Accordion ID="AccRecipientInformation" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">
    <Panes>
        <ajax:AccordionPane ID="AccoPaneRecipientInformation" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblAP3" class="expandcollapse" runat="server" Text="- * RECIPIENT INFORMATION"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center;">
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label" style="text-align: right;"><b>Medicaid Billing Number : </b>
                            <asp:Label ID="lblMedicaidBillingNumber" runat="server">
                            </asp:Label>
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label" style="text-align: right;"><b>Date of Birth : </b>
                            <asp:Label ID="lblDateofBirth" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label" style="text-align: right;"><b>Submission Date : </b>
                            <asp:Label ID="lblSubmissionDate" runat="server"></asp:Label></span>
                    </div>
                </div>
                <div class="row" style="text-align: center;">
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label" style="text-align: right;"><b>Last Name : </b>
                            <asp:Label ID="lblLastName" runat="server">
                            </asp:Label>
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3">
                        <span class="ohio-field-label" style="text-align: right;"><b>Street Address : </b>
                            <asp:Label ID="lbleStreetAddress" runat="server"></asp:Label>
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label" style="text-align: right;"><b>County of Record : </b>
                            <asp:Label ID="lblCountyofRecord" runat="server"></asp:Label>
                        </span>
                    </div>
                </div>
                <div class="row" style="text-align: center;">
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label" style="text-align: right;"><b>First Name, MI : </b>
                            <asp:Label ID="lblFirstname" runat="server">
                            </asp:Label>
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-4">
                        <span class="ohio-field-label" style="text-align: right;"><b>City, State, and Zip Code : </b>
                            <asp:Label ID="lblCityStateZip" runat="server"></asp:Label>
                        </span>
                    </div>
                </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>
