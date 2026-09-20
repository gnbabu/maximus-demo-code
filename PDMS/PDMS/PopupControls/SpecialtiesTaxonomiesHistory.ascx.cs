using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_SpecialtiesTaxonomiesHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private bool HideIndexColumn()
    {
        bool rtn = false;
        int cnt = 0;
        int saveIdx = 0;
        foreach (GridViewRow row in grdSpecialtiesTaxonomiesHistory.Rows)
        {
            int idx = string.IsNullOrEmpty(row.Cells[0].Text)?0:Convert.ToInt32(row.Cells[0].Text);

            if (idx != saveIdx)
            {
                saveIdx = idx;
                cnt += 1;
            }
        }
        if (cnt <= 1) rtn = true;
        return rtn;
    }

    public void LoadData(int primaryFlag)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", primaryFlag.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTY_TAXONOMYHistory", parms);
        if (Helper.HasRows(ds))
        {
            grdSpecialtiesTaxonomiesHistory.DataSource = ds.Tables[0];
            grdSpecialtiesTaxonomiesHistory.DataBind();
            //if (HideIndexColumn()) grdSpecialtiesTaxonomiesHistory.Columns[0].Visible = false;
        }
    }
}