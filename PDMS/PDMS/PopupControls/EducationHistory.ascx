<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_EducationHistory" Codebehind="EducationHistory.ascx.cs" %>
<br />
<asp:gridview runat="server" width="98%" id="grdEducationHistory" autogeneratecolumns="False" horizontalalign="Left"
    cssclass="gridview" emptydatatext="No entries found.">
    <columns>
        <asp:boundfield datafield="SCHOOL"                  headertext="School" />
        <asp:boundfield datafield="display_name"            headertext="Education Type" />
        <asp:boundfield datafield="FIELDOFSTUDY"            headertext="Field Of Study" />
        <asp:boundfield datafield="Start_Year"              headertext="Start Date" dataformatstring="{0:MM/dd/yyyy}" />
        <asp:boundfield datafield="end_year"                headertext="End date" dataformatstring="{0:MM/dd/yyyy}" />
        <asp:boundfield datafield="LAST_MODIFIED_USERNAME"  headertext="User Name" />
        <asp:boundfield datafield="LAST_MODIFIED_DATE_TIME" headertext="Update" dataformatstring="{0:d}" htmlencode="False"  />
    </columns>
    <pagerstyle 			cssclass="gridpager" 		horizontalalign="Right" />
    <headerstyle 			cssclass="gridViewHeader" 	width="100px" />
    <alternatingrowstyle 	cssclass="gridViewAltRow" />
    <rowstyle 				cssclass="gridViewRow" />
    <footerstyle cssclass="gridViewFooter" />
</asp:gridview>
