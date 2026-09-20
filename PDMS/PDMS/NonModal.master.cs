using System;
using System.Web.UI;

public partial class NonModal_MasterPage : BaseMasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
        }

        Page.Header.DataBind();
    }

  
}
