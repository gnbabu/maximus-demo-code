using System;
using System.Web;

public partial class Account_MyProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect(@"~/Process/AdminUserAccounts.aspx?username=" + HttpContext.Current.User.Identity.Name);
    }
}