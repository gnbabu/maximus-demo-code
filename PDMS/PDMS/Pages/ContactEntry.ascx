<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_ContactEntry" Codebehind="ContactEntry.ascx.cs" %>

<table>
<tr><td>

<div id="div_ContactPractice">
<b>Practice Contact Information</b>
<table class="gridView" cellspacing="0" rules="all" align="left" border="1" id="Table1" style="border-width:1px;border-style:Solid;width:75%;border-collapse:collapse; margin-left:12%;">
<tr><td>
<table>
<tr><td align="right">First Name</td><td align="left"><input id="Text2" /></td></tr>
<tr><td align="right">Last Name</td><td align="left"><input id="Text1" /></td></tr>
<tr><td align="right">Phone Number</td><td align="left"><input id="Text3" /></td></tr>
<tr><td align="right">Phone Number Ext</td><td align="left"><input id="Text4" /></td></tr>
<tr><td align="right">Fax Number</td><td align="left"><input id="Text5" /></td></tr>
<tr><td align="right">Email Address</td><td align="left"><input id="Text6" /></td></tr>
<tr><td align="right">
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBox" 
        onclick="btnSave_Click" />&nbsp;
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" 
        onclick="btnCancel_Click" />
</td></tr>
</table>
</td></tr>
</table>
</div>

</td></tr>
<tr><td>

<div id="div_ContactBilling">
<b>Billing Contact / Location Information</b>
<table class="gridView" cellspacing="0" rules="all" align="left" border="1" id="Table2" style="border-width:1px;border-style:Solid;width:75%;border-collapse:collapse; margin-left:12%;">
<tr><td>
<table>
<tr><td align="right">Billing Contact Person First Name</td><td align="left"><input id="Text7" /></td></tr>
<tr><td align="right">Billing Contact Person Last Name</td><td align="left"><input id="Text8" /></td></tr>
<tr><td align="right">Phone Number</td><td align="left"><input id="Text9" /></td></tr>
<tr><td align="right">Phone Number Ext</td><td align="left"><input id="Text10" /></td></tr>
<tr><td align="right">Fax Number</td><td align="left"><input id="Text11" /></td></tr>
<tr><td align="right">Email Address</td><td align="left"><input id="Text12" /></td></tr>
<tr><td align="right">Billing Address</td><td align="left"><input id="Text19" /></td></tr>
<tr><td align="right">Address 2</td><td align="left"><input id="Text20" /></td></tr>
<tr><td align="right">City</td><td align="left"><input id="Text21" /></td></tr>
<tr><td align="right">State</td><td align="left"><input id="Text22" /></td></tr>
<tr><td align="right">Zip</td><td align="left"><input id="Text23" /></td></tr>
<tr><td align="right">Ext Zip</td><td align="left"><input id="Text24" /></td></tr>
<tr><td align="right"><a href="#">Save</a></td><td align="left"><a href="#">Cancel</a></td></tr>
</table>
</td></tr>
</table>
</div>

</td></tr>
<tr><td>

<div id="div_ContactPayment">
<b>Payment Contact / Location Information</b><br />
<i>Payments for this service location will be made out to the Pay To Name and remittance advices will be sent the address entered below.</i>
<table class="gridView" cellspacing="0" rules="all" align="left" border="1" id="Table3" style="border-width:1px;border-style:Solid;width:75%;border-collapse:collapse; margin-left:12%;">
<tr><td>
<table>
<tr><td align="right">Pay-To Name</td><td align="left"><input id="Text13" /></td></tr>
<tr><td align="right">Payment Contact Person First Name</td><td align="left"><input id="Text25" /></td></tr>
<tr><td align="right">Payment Contact Person Last Name</td><td align="left"><input id="Text14" /></td></tr>
<tr><td align="right">Phone Number</td><td align="left"><input id="Text15" /></td></tr>
<tr><td align="right">Phone Number Ext</td><td align="left"><input id="Text16" /></td></tr>
<tr><td align="right">Fax Number</td><td align="left"><input id="Text17" /></td></tr>
<tr><td align="right">Email Address</td><td align="left"><input id="Text18" /></td></tr>
<tr><td align="right">Payment / Remittance Address</td><td align="left"><input id="Text26" /></td></tr>
<tr><td align="right">Address 2</td><td align="left"><input id="Text27" /></td></tr>
<tr><td align="right">City</td><td align="left"><input id="Text28" /></td></tr>
<tr><td align="right">State</td><td align="left"><input id="Text29" /></td></tr>
<tr><td align="right">Zip</td><td align="left"><input id="Text30" /></td></tr>
<tr><td align="right">Ext Zip</td><td align="left"><input id="Text31" /></td></tr>
<tr><td align="right"><a href="#">Save</a></td><td align="left"><a href="#">Cancel</a></td></tr>
</table>
</td></tr>
</table>
</div>

</td></tr>
</table>