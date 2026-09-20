using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SearchClaims : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnUserName.Value = HttpContext.Current.User.Identity.Name;
    }
}