<%@ control language="C#" autoeventwireup="true" inherits="Pages_SupportingDocumentation, App_Web_roucadzr" %>

<table width="100%">
<tr><td>
    <table class="gridView" cellpadding="4" rules="all" align="left" border="1" id="Table11">
        <tr class="gridViewHeader">
        <th scope="col">Document Name</th>
        <th scope="col">&nbsp;</th>
        </tr>
        <tr class="gridViewRow">
            <td align="left">Driver's License</td>
            <td align="center"><asp:Image ID="imgDelete" runat="server" ImageUrl="~/Images/delete2.png" /></td>
        </tr>
        <tr class="gridViewAltRow">
            <td align="left">Fishing License</td>
            <td align="center"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/delete2.png" /></td>
        </tr>
        <tr class="gridViewRow">
            <td align="left">Passport</td>
            <td align="center"><asp:Image ID="Image2" runat="server" ImageUrl="~/Images/delete2.png" /></td>
        </tr>
        <tr class="gridViewRow">
        <td align="left" colspan="2"><a id="A15" href="#">Upload New Document</a></td>
        </tr>
    </table>
</td></tr>
</table>