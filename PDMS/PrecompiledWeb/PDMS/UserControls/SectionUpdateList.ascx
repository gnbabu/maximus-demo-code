<%@ control language="C#" autoeventwireup="true" inherits="UserControls_SectionUpdateList, App_Web_p4ixifjm" %>

<div class="sectionHeader" style="width: 100%;">
                    <table class="tableSection">
                        <tr>
                            <td width="150px;">&nbsp;</td>
                            <td style="text-align: left">
                                <asp:Label ID="sectionName" runat="server" CssClass="formLabelAuto" Text="Provider Name" /></td>   
                            <td>&nbsp;</td>                                                     
                        </tr>
                        <tr>
                            <td width="150px">
                                <asp:Image runat="server" ID="sectionIcon" ImageUrl="~/Images/icon.jpg" CssClass="pageIcon" ></asp:Image>
                            </td>
                            <td>
                    <asp:GridView runat="server" ID="secListGrid" AutoGenerateColumns="false" ShowHeader="false" BorderWidth="0" OnRowDataBound="secListGrid_RowDataBound" CssClass="noBorderGrid" AlternatingRowStyle-CssClass="noBorderGridAltRow" RowStyle-CssClass="noBorderGridRow" OnRowUpdating="secListGrid_RowUpdating">
                        <Columns>
                                <%--<asp:BoundField DataField="SECTION_DISPLAY_NAME" HeaderText="Section Name" ItemStyle-CssClass="boundFieldCss" />--%>
                                <asp:TemplateField HeaderText="Action" >
                                    <ItemTemplate>
                                        <asp:Button ToolTip="Update" ID="btnUpdate" alt="ProvideInformationLogo..." CssClass="buttonUpdate" runat="server" CommandName="Update" CommandArgument='<%# Eval("REG_SECTION_TYPE_ID") %>' OnClick="btnUpdate_Click" PostBackUrl="~/Process/Registration.aspx"
                                            Text="Update" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SECTION_DISPLAY_NAME" HeaderText="Section Name" ItemStyle-CssClass="boundFieldCss" />
                                <asp:TemplateField HeaderText="Status" >
                                    <ItemTemplate>
                                        <asp:Image ID="statusImg" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                                </td></tr>
                        </table>
</div>
