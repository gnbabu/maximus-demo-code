using System;
using System.Web.UI;

public partial class Process_CallTracker : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitPage();
        }

    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        { 
            Page.Theme = "Modernization";
           
        }
        else
        {
            Page.Theme = "Default";
        }
    }

    #region Private Methods

    private void InitPage()
    {
        ucCallAddView.InitView();
    }
    #endregion
}