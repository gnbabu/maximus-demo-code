using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SearchMemberEligibility : System.Web.UI.UserControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        hdnUserName.Value = HttpContext.Current.User.Identity.Name;
        if (Session["MedID"] != null)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["MedID"])))
            {
                hdnMedicaidID.Value = Convert.ToString(Session["MedID"]);
            }
        }
        else
        {
            hdnMedicaidID.Value = this.WorkflowPage.MedicaidID;
        }

    }
}