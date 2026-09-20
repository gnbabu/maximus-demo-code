<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Education" Codebehind="Education.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/EducationHistory.ascx" TagPrefix="uc" TagName="EducationHistory" %>

<script src="../Scripts/jquery.inputmask.bundle.min.js"></script>         
<script type="text/javascript">
     $(document).ready(function(){
        $('.phone_number').inputmask('(999) 999-9999');
     });

    $(document).keydown(function (e) {
        // ESCAPE key pressed
        if (e.keyCode == 27) {
            $find("mpeok").hide();
            document.getElementById("<%=btnEducationHistory.ClientID %>").focus();
            return false;
        }

        var target = e.target;
        var shiftPressed = e.shiftKey;
        // If TAB key pressed
        if (e.keyCode == 9) {                 // If inside a Modal dialog (determined by attribute role="dialog")
            if ($(target).parents('[role=dialog]').length) {
                // Find first or last input element in the dialog parent (depending on whether Shift was pressed). 
                // Input elements must be visible, and can be Input/Select/Button/Textarea.
                var borderElem = shiftPressed ?
                    $(target).closest('[role=dialog]').find('input:visible,select:visible,button:visible,textarea:visible').first()
                    :
                    $(target).closest('[role=dialog]').find('input:visible,select:visible,button:visible,textarea:visible').last();
                if ($(borderElem).length) {
                    if ($(target).is($(borderElem))) {
                        return false;
                    } else {
                        return true;
                    }
                }
            }
        }
        return true;
    });
</script>

<div>
    <asp:ValidationSummary ID="vsEducation" runat="server" DisplayMode="List" ValidationGroup="vgEducation" />
</div>
<div style="width: 100%;">
    <div class="row">
        <div class="col-sm-9">
            <div class="pg-hint4">
                <span>Please enter all education and training you have completed beginning with your undergraduate degree through your professional education and training.</span>
            </div>
        </div>
    </div>
</div>
<asp:Panel runat="server" ID="pnlEducationGrid">
    <div class="divGrid">
        <asp:GridView runat="server" ID="grdEducation" Caption="<span style='display:none'>Education</span>"  AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grdEducation_RowCommand">
            <Columns>
                <asp:BoundField DataField="SCHOOL" HeaderText="School" />
                <asp:BoundField DataField="EDUCATION_TYPE" HeaderText="Education" />
                <asp:BoundField DataField="SPECIALTY_NAME" HeaderText="Specialty" />
                <asp:BoundField DataField="DEGREE_ABBREV" HeaderText="Degree" />
                <asp:BoundField DataField="START_YEAR" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="END_YEAR" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:TemplateField ItemStyle-Width="2%" Headertext="Edit">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditEducation" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
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
        <asp:ImageButton ID="btnAddEducationItem" AlternateText="addnew" runat="server" ImageUrl="~/Images/add.png" CommandName="EducationAdd" OnCommand="btnAddEducationItem_Click" ToolTip="Add" /><br />
        <asp:linkbutton ID="btnEducationHistory" CommandName="EducationHistory" runat="server" CssClass="buttonBoxFocus" OnCommand="btnEducationHistory_Click" ToolTip="History" style="color: white; text-decoration: none;">
            <span class="glyphicon glyphicon-book" style="padding-right: 7px;"></span>History 
        </asp:linkbutton>

    </div>
    <br />
</asp:Panel>
<asp:UpdatePanel ID="updPnlEducationEntry" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:Panel runat="server" ID="pnlEducationEntry" Visible="false">
            <div style="width: 100%;">
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="Label1" runat="server" Text="*Education Type:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlEducationType" CssClass="formDropDown" aria-label="Education Type"
                            OnSelectedIndexChanged="ddlEducationType_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RfvddlEducationType" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="ddlEducationType" ErrorMessage="*Enter Education Type" Text="*" Display="Dynamic" InitialValue="0" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblSchool" runat="server" Text="*Name Of School:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbSchool" runat="server" aria-label="Name Of School" CssClass="formField"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvSchool" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="tbSchool" ErrorMessage="*Enter School" Text="*" Display="Dynamic" />
                    </div>
                </div>

                <%--   <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="lblFieldOfStudy" runat="server" Text="*Field Of Study:" CssClass="formLabel wd200"></asp:Label>
            </div>
            <div class="col-sm-8">
            <asp:TextBox ID="tbFieldStudy" runat="server" CssClass="formField"></asp:TextBox>
                 <asp:RequiredFieldValidator runat="server" ID="rfvFieldStudy" SetFocusOnError="true" 
            ValidationGroup="vgEducation" ControlToValidate="tbFieldStudy" ErrorMessage="*Enter Field of Study" Text="*" Display="Dynamic"  />
                </div>
    </div>--%>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblStartYear" runat="server" Text="*Start Date:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbStartYear" runat="server" aria-label="Start Date"  CssClass="formField"></asp:TextBox>
                        <ajax:CalendarExtender ID="calExtenderStartYear" TargetControlID="tbStartYear" runat="server" />
                        <asp:CompareValidator 
                            id="tbStartYearValidator" 
                            runat="server"  
                            Type="Date" 
                            Operator="DataTypeCheck" 
                            ControlToValidate="tbStartYear"  
                            ErrorMessage="Select a valid From date" 
                            Text="*" 
                            Display="Dynamic" 
                            ValueToCompare="MM/dd/yyyy"
                            ValidationGroup="vgEducation"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblEndYear" runat="server" Text="*End Date:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbEndYear" runat="server" aria-label="End Date" CssClass="formField"></asp:TextBox>
                        <ajax:CalendarExtender ID="calExtenderEndYear" TargetControlID="tbEndYear" runat="server" />
                        <asp:CompareValidator 
                            id="tbEndYearvalidator" 
                            runat="server"  
                            Type="Date" 
                            Operator="DataTypeCheck" 
                            ControlToValidate="tbEndYear"  
                            ErrorMessage="Select a valid To date" 
                            Text="*" 
                            Display="Dynamic" 
                            ValueToCompare="MM/dd/yyyy"
                            ValidationGroup="vgEducation"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblDegreeAwarded" runat="server" Text="*Degree/ Certificate Awarded:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlDegreeAward" CssClass="formDropDown" aria-label="Degree/ Certificate Awarded"
                            OnSelectedIndexChanged="ddlDegreeAward_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="ddlDegreeAward" ErrorMessage="*Degree Awarded" Text="*" Display="Dynamic" InitialValue="0" />
                    </div>
                </div>                
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="Label2" runat="server" Text="Speciality:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:DropDownList runat="server" ID="ddlSpeciality" aria-label="Speciality" CssClass="formDropDown" AppendDataBoundItems="True">
                        </asp:DropDownList>
                    </div>
                </div>  
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblAddress1" runat="server" Text="*Address 1:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbAddress1" aria-label="Address 1" runat="server" CssClass="formField"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="tbAddress1" ErrorMessage="*Enter Address1" Text="*" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblAddress2" runat="server" Text="Address 2:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbAddress2" aria-label="Address 2" runat="server" CssClass="formField"></asp:TextBox>
                    </div>
                </div>              
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblCity" runat="server" Text="*City:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbCity" aria-label="City" runat="server" CssClass="formField"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvCity" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="tbCity" ErrorMessage="*Enter City" Text="*" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblState" runat="server" Text="*State:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlState" aria-label="State" runat="server" CssClass="formDropDown" AutoPostBack="true" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator runat="server" ID="RfvState" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="ddlState" ErrorMessage="*Enter State" Text="*" Display="Dynamic" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblZipCode" runat="server" Text="* Zip Code:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbZipCode" aria-label="Zip Code"  runat="server" CssClass="formField"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ID="RfvZip" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="tbZipCode" ErrorMessage="*Enter Zip" Text="*" Display="Dynamic" />
                        <asp:RegularExpressionValidator runat="server" ID="revZip" ValidationExpression="^[0-9]*$" ValidationGroup="vgEducation" ControlToValidate="tbZipCode" ErrorMessage="*Enter Valid Zip" Text="*" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblCountry" runat="server" Text="*Country:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                          <asp:DropDownList runat="server" aria-label="Country" ViewStateMode="Enabled" onchange="Page_BlockSubmit = false;"  EnableViewState="true" ID="ddlCountry" CssClass="formDropDown" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
<%--                      <asp:TextBox ID="tbCountry" runat="server" CssClass="formField" OnTextChanged="tbCountry_TextChanged" ></asp:TextBox>--%>
                        <asp:RequiredFieldValidator runat="server" ID="RfvCountry" SetFocusOnError="true"
                            ValidationGroup="vgEducation" ControlToValidate="ddlCountry" ErrorMessage="*Enter Country" Text="*" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblPhone" runat="server" Text="Phone Number:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbPhone" aria-label="Phone Number" runat="server" CssClass="formField phone_number"></asp:TextBox>                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblFax" runat="server" Text="Fax:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbFax" aria-label="Fax"  runat="server" CssClass="formField phone_number"></asp:TextBox>                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4  text-right">
                        <asp:Label ID="lblAdditional" runat="server" Text="Additional Information:" CssClass="formLabel wd200"></asp:Label>
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="tbAdditional" aria-label="Additional Information"  runat="server" TextMode="MultiLine" CssClass="formField"></asp:TextBox>
                    </div>
                </div>

            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hdnRegEducationId" runat="server" />
 <div role="dialog" aria-labelledby="h2education" aria-modal="true" aria-live="assertive">
<ajax:ModalPopupExtender ID="mpeEducationHistory" runat="server" PopupControlID="pnlModalEducation" BehaviorID="mpeok" TargetControlID="ButtonDummy" CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModalEducation" runat="server" CssClass="modalPopup" Style="display: none; width: 50%; height: auto;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <div class="popTitle">
            <h2 id="h2education"><asp:Label ID="lbl_title" runat="server" Text="Title" /></h2>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwEducationHistory" runat="server">
                <div style="text-align: left; padding: 15px; margin-left: 30px !important" class="container-fluid">
                    <div class="row">
                        <uc:EducationHistory ID="ucEducationHistory" runat="server" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <div class="btnBox" style="padding: 10px">
        <asp:Button runat="server" ID="btnCancel" Text="OK" CssClass="buttonBox" CausesValidation="false" style="margin-right:10px;" />
    </div>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
     </div>