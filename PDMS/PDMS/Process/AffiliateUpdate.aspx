<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_AffiliateUpdate" Codebehind="AffiliateUpdate.aspx.cs" %>

<%@ register src="~/PopupControls/UploadFile.ascx" tagprefix="ucUF" tagname="UploadFile" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/PopupControls/MessageModal.ascx" tagname="MessageBox" tagprefix="uc" %>
<%@ register src="~/PopupControls/MessageBox.ascx" tagname="MessageBox" tagprefix="cc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageLabelContent" runat="Server">
    Affiliate Update 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">
        <div  class="col-12 text-right" >
            <p>
                <asp:LinkButton ID="lbTemplate" runat="server" Text="Please download the affiliate <br /> update template here" OnClick="lbAffliateUpdate_Click" />
                 <img src="../Images/Excel_24x24.png" style="margin-left:4px;" alt="Please download the affiliate update template here"/>
            </p>
        </div>
        <div class="col-lg-3 col-sm-0 hidden-xs hidden-sm">
            <asp:Image runat="server" ID="sectionIcon" ImageUrl="~/Images/ProviderInformation_Lg.png" Style="margin-left: 30%; margin-top: 50%;" CssClass="img-responsive"></asp:Image>
        </div>

        <div class="col-lg-9 col-sm-12">
            <div class="divGrid">
                <asp:GridView ID="grdAffiliateUpdate" Width="100%" runat="server" AllowSorting="true" OnSorting="grdAffiliateUpdate_SortCommand" CssClass="gridview" EmptyDataText="No Affiliates found"
                    AutoGenerateColumns="false" HorizontalAlign="Left" OnRowCommand="grdAffiliateUpdate_RowCommand" AllowPaging="True" PageSize="12"
                    OnPageIndexChanging="grdAffiliateUpdate_PageIndexChanging" OnRowDataBound="grdAffiliateUpdate_RowDataBound" ShowHeaderWhenEmpty="true"
                    DataKeyNames="DELEGATE_DOCUMENT_UPLOAD_ID,Request_File,Response_File">
                    <columns>
                        <asp:BoundField DataField="Affiliate_File_Upload_Date" HeaderStyle-Font-Underline="true" HeaderText="Affiliate File Upload Date" SortExpression="Affiliate_File_Upload_Date" />
                        <asp:BoundField DataField="Request_File" HeaderText="Affiliate File Name" />
                        <asp:BoundField DataField="Affiliate_File_Upload_Status" HeaderText="Affiliate File Upload Status" HeaderStyle-Font-Underline="true" SortExpression="Affiliate_File_Upload_Status" />
                        <asp:BoundField DataField="DELEGATE_DOCUMENT_UPLOAD_ID" HeaderText="Affiliate Row ID" Visible="false" />
                        <asp:TemplateField HeaderText="Upload" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                            <asp:LinkButton  ID="lnkRequestFile" 
                            runat="server" 
                            CausesValidation="false" 
                            Text="View Upload"
                            CommandName="upload"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            CssClass="gridLink" Width="150" HeaderText="Upload"/>                                 
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Response" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                        <asp:LinkButton ID="lnkResponsePath" 
                            runat="server" 
                            CausesValidation="false" 
                            Text='<%# Eval("Response_File").ToString()!=""?"View Response" : "" %>'
                            CommandName="response"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            CssClass="gridLink" Width="150"
                            Visible='<%# Eval("Response_File").ToString()==""?false : true %>' />                          
                    </ItemTemplate>
                </asp:TemplateField>
                    </columns>
                    <pagerstyle cssclass="gridpager" horizontalalign="Right" />
                    <headerstyle cssclass="gridViewHeader" width="100px" />
                    <alternatingrowstyle cssclass="gridViewAltRow" />
                    <rowstyle cssclass="gridViewRow" />
                    <footerstyle cssclass="gridViewFooter" />
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="panel" Width="100%" HorizontalAlign="Center">
            </asp:Panel>
        </div>
        <br />
        <br />
        <br />
        <br />
        <div class="divHistoryAndAdd">
            <asp:Button ID="btnUploadFile" runat="server" CommandName="UploadFile" OnCommand="btnUploadFile_Click" ToolTip="Upload File" Text="Upload File" CssClass="buttonBox" />
        </div>
        <br />
        <br />
        <div class="col-lg-3 col-sm-0 hidden-xs hidden-sm">&nbsp;</div>


        <div class="col-lg-9 col-sm-12">

            <table class="table">


                <tr style="background-color:darkslateblue">
                    <th>&nbsp;</th>
                    <th style="color:snow;" >Affiliate Update Error Code Definitions</th>
                </tr>

                <tr>
                    <th>Error Code</th>
                    <th>Definition</th>
                </tr>

                <tr>
                    <td>DA001</td>
                    <td>Affiliated Provider is not present/enrolled in the system</td>
                </tr>


                <tr>
                    <td>DA002</td>
                    <td>Affiliated Provider is not active in the system</td>
                </tr>


                <tr>
                    <td>DA003</td>
                    <td>Affiliated Group is not present/enrolled in the system</td>
                </tr>

                <tr>
                    <td>DA004</td>
                    <td>Affiliated Group is not active in the system</td>
                </tr>


                <tr>
                    <td>DA005</td>
                    <td>Affiliated Provider at this rendering location is a duplicate</td>
                </tr>

                <tr>
                    <td>DA006</td>
                    <td>Affiliation start date cannot be before the Group’s start date</td>
                </tr>


                <tr>
                    <td>DA007</td>
                    <td>Affiliation start date cannot be changed</td>
                </tr>


                <tr>
                    <td>DA008</td>
                    <td>Affiliation end date in system cannot be earlier than Affiliations start date</td>
                </tr>

                <tr>
                    <td>DA009</td>
                    <td>Affiliated Provider’s NPI and Medicaid ID do not match</td>
                </tr>


                <tr>
                    <td>DA010</td>
                    <td>Affiliation End Date cannot be after the Group’s end date</td>
                </tr>

                <tr>
                    <td>DA011</td>
                    <td>Affiliated Provider is already end dated at this rendering location</td>
                </tr>


                <tr>
                    <td>DA012</td>
                    <td>An Individual Provider cannot have affiliations</td>
                </tr>


                <tr>
                    <td>DA013</td>
                    <td>A Group Provider cannot be affiliated with another Group</td>
                </tr>

                <tr>
                    <td>DA014</td>
                    <td>Required data elements are missing, the upload could not be completed</td>
                </tr>


                <tr>
                    <td>DA015</td>
                    <td>You currently do not have the authorization to update the Group Provider</td>
                </tr>


                <tr>
                    <td>DA016</td>
                    <td>Rendering Location does not exist for this provider</td>
                </tr>


                <tr>
                    <td>DA017</td>
                    <td>Data in this row is incorrectly formatted. Please re-download template and try again</td>
                </tr>

                <tr>
                    <td>DA017.A</td>
                    <td>Update Type column value should be A or E</td>
                </tr>

                <tr>
                    <td>DA017.B</td>
                    <td>Group Medicaid Id column should be numeric and 10 characters or less</td>
                </tr>

                <tr>
                    <td>DA017.C</td>
                    <td>Affiliate Medicaid Id column should be numeric and 10 characters or less</td>
                </tr>

                <tr>
                    <td>DA017.D</td>
                    <td>Affiliate provider’s NPI should be exact 10 characters</td>
                </tr>

                <tr>
                    <td>DA017.E</td>
                    <td>Affiliate Start Date required and should be in MM/DD/YYYY format</td>
                </tr>

                <tr>
                    <td>DA017.F</td>
                    <td>Affiliate End Date should be in MM/DD/YYYY format</td>
                </tr>

                <tr>
                    <td>DA017.G</td>
                    <td>Rendering Location Address Line1 is required and should not exceed more than 60 characters</td>
                </tr>

                <tr>
                    <td>DA017.H</td>
                    <td>Rendering Location Address Line2 should not exceed more than 60 characters if provided</td>
                </tr>

                <tr>
                    <td>DA017.I</td>
                    <td>Rendering Location Address City is required and should not exceed more than 30 characters</td>
                </tr>

                <tr>
                    <td>DA017.J</td>
                    <td>Rendering Location Address State is required and should not exceed more than 30 characters</td>
                </tr>

                <tr>
                    <td>DA017.K</td>
                    <td>Rendering Location Address Zip is required and should be 5 characters</td>
                </tr>

                <tr>
                    <td>DA018</td>
                    <td>Phone number not in the expected format</td>
                </tr>

                <tr>
                    <td>DA019</td>
                    <td>When adding an affiliate the individual affiliation start date cannot be prior to the group’s effective date</td>
                </tr>
                 <tr>
                     <td>DA020</td>
                     <td>Affiliate provider start date cannot be prior to the individual’s effective date</td>
                 </tr>

                 <tr>
                     <td>DA021</td>
                     <td>Affiliated provider start date cannot be prior to the individual’s conversion to standard effective date</td>
                 </tr>
                <tr>
                    <td>DA022</td>
                    <td>Affiliation start date or rendering location does not match an existing record.</td>
                </tr>

                <tr>
                    <td>DA023</td>
                    <td>End date of rendering location is prior to the start date of provider at this location.</td>
                </tr>

                <tr>
                    <td>DA024</td>
                    <td>End date of providers affiliation at the rendering location is greater than the end date of the location.</td>
                </tr>
            </table>

        </div>
    </div>




    <cc1:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy" backgroundcssclass="modalBackground" />
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 300px; min-width: 900px; height: auto; width: auto;">
        <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
            <div class="popTitle">
                <asp:Label ID="lblTitle" CssClass="bodyTextBold hospitalAffiliationsTitle" runat="server" Text="Title" />
            </div>
        </asp:Panel>
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <ucuf:uploadfile id="ucUploadFile" runat="server" />
            </div>
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
    <cc2:messagebox id="MessageBox2" runat="server" />
</asp:Content>