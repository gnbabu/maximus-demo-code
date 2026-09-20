using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WorkflowPage
/// </summary>
public class WorkflowPage : System.Web.UI.Page
{
    override protected void OnInit(EventArgs e)
    {
        this.PreRender += new System.EventHandler(this.Page_PreRender);
        base.OnInit(e);
    }

    virtual protected void Page_PreRender(object sender, EventArgs e)
    {
    }
}
