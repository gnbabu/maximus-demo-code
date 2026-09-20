<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ManyToManySelector, App_Web_c4une0e1" %>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td><asp:ListBox ID="lbSource" runat="server" SelectionMode="Multiple" Height="250" Width="250" /></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnSourceAll" class="listButtonsBelow btn btn-default btn-xs active" runat="server" Text="All" OnClick="btnSourceAll_Click" />
                                    <asp:Button ID="btnSourceNone" class="listButtonsBelow btn btn-default btn-xs active" runat="server" Text="None" OnClick="btnSourceNone_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="25%" align="center">
                        <asp:Button ID="btnAddToSelected" runat="server" Text="Add >>>" class="listButtons btn btn-default btn-xs active" style="font-size:8pt;width:100px;" OnClick="btnAddToSelected_Click" />
                        <br />
                        <br />
                        <asp:Button ID="btnRemoveFromSelected" class="listButtons btn btn-default btn-xs active" runat="server" Text="<<< Remove" style="font-size:8pt;width:100px;" OnClick="btnRemoveFromSelected_Click" />
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td><asp:ListBox ID="lbSelected" runat="server" SelectionMode="Multiple" Height="250" Width="250" /></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button class="listButtonsBelow btn btn-default btn-xs active" ID="btnSelectedAll" runat="server" Text="All"  OnClick="btnSelectedAll_Click" />
                                    <asp:Button class="listButtonsBelow btn btn-default btn-xs active" ID="btnSelectedNone" runat="server" Text="None" OnClick="btnSelectedNone_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
