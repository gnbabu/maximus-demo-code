<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_FormW9" Codebehind="FormW9.ascx.cs" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>

<asp:ValidationSummary ID="vsFormW9" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Formw9" /> 
<div id="divInstructions" runat="server" style="text-align: left;">
    <asp:Label runat="server" Text="Information from the Identification page displayed below." style="width: 100%;"/> <br/>
    <asp:Label runat="server" Text="Corrections to this information must be made in Organization/Individual Identification and Primary Contact sections of the Identification page." style="width: 100%; font-style: italic;"/>
</div> 
<br/>
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row" id="divTaxArea" runat="server" style="width: 100%;">
            <div class="col-sm-4  text-right">
                <asp:Label ID="lblOrgName" runat="server" Text="Legal Business Name:" CssClass="formLabel200"/>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtOrgName" runat="server" CssClass="formLabel" aria-Label="Org Name" style="text-align: left; width: 450px;" Enabled="False"/>
            </div>
            <div style="display: none;">
                <asp:Label ID="Label1" runat="server"/>
            </div>
            <div id="divIndividual" runat="server">
                <div style="display: none;">
                    <asp:Label ID="Label2" runat="server"/>
                </div>
                <div class="col-sm-4  text-right" runat="server">
                    <asp:Label ID="lblSSN" runat="server" CssClass="formLabel200" Text="SSN:"/>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtSSN" runat="server" CssClass="formLabel" style="text-align: left; width: 450px;" aria-Label="SSN" Enabled="False"/>
                </div>
            </div>
            <div id="divOrganization" runat="server">
                <div style="display: none;">
                    <asp:Label ID="Label10" runat="server"/>
                </div>
                <div class="col-sm-4  text-right" runat="server">
                    <asp:Label ID="lblEIN" runat="server" CssClass="formLabel200" Text="EIN:"/>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtEIN" runat="server" CssClass="formLabel" style="text-align: left; width: 450px;" Enabled="False"/>
                </div>
            </div>
            <div style="display: none;">
                <asp:Label ID="Label3" runat="server"/>
            </div>
            <div style="margin-left: 250px;">
                <asp:Repeater ID="rptTaxClassification" runat="server" OnItemDataBound="rptTaxClassification_OnItemDataBound">
                    <HeaderTemplate>
                        <div class="legend">
                            <asp:Label runat="server" ID="lblCategory">Select the most appropriate category below:</asp:Label>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row" style="margin-left: 120px;">
                            <asp:RadioButton runat="server" aria-label='<%# DataBinder.Eval(Container.DataItem, "[TAX_ENTITY_TYPE]") %>' ID="rbTaxClass" CssClass="QstRadioList" RepeatDirection="Horizontal"
                                             OnCheckedChanged="rbTaxClass_OnCheckedChanged" AutoPostBack="True"/>
                            <asp:Label runat="server" ID="lbTaxId" Text='<%# DataBinder.Eval(Container.DataItem, "[TAX_ENTITY_TYPE_ID]") %>' Visible="false"/>
                            <asp:Label AssociatedControlID="rbTaxClass" runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem, "[TAX_ENTITY_TYPE]") %>'/>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="col-sm-4  text-right">
                <span class="formLabel200">Indicate the form you are uploading</span>
            </div>
            <br />
            <div class="row" style="margin-left: 100px;">
                <fieldset>
                    <legend>
                 <asp:RadioButtonList ID="rbIndicate" runat="server" CssClass="QstRadioList"  RepeatDirection="Vertical" ValidationGroup="Formw9" >
                    <asp:ListItem Value="1">W9</asp:ListItem>
                    <asp:ListItem Value="2">Form 147</asp:ListItem>
                </asp:RadioButtonList>
                    </legend>
               </fieldset>
                 <asp:RequiredFieldValidator ID="rbIndicate1" runat="server" ControlToValidate="rbIndicate" 
                      SetFocusOnError="true"
                          Display="Dynamic" text="*" ErrorMessage="Upload Form Section is Required" ValidationGroup="Formw9"></asp:RequiredFieldValidator>
                  </div>
            <br/>
            <div class="row">
                ** Please visit <a href="https://www.irs.gov/forms-pubs/about-form-w-9">https://www.irs.gov/forms-pubs/about-form-w-9</a> to obtain a copy of the W9 with instructions.
            </div>
            <br />
            <asp:PlaceHolder runat="server" ID="PlaceholderUploadW9"></asp:PlaceHolder>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false"/>
<asp:TextBox ID="hidID" runat="server" Visible="false"/>
<asp:TextBox ID="hidTaxInfo" runat="server" Visible="false"/>