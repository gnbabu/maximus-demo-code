using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private void LoadGrid(string tableName, GridView grd)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        if (tableName == "SUBCONTRACTOR" || tableName == "SUBCONTRACTOR5YRS")
        {
            int subTypeId = CON.RegistrationSubcontractorType.OwnershipAtLeast5Percent;
            if (tableName == "SUBCONTRACTOR5YRS") subTypeId = CON.RegistrationSubcontractorType.BusinessDoneLast5Years;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("SUBCONTRACTOR_TYPE_ID", subTypeId.ToString());
            ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SUBCONTRACTOR_History", parms);
        }
        else ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, tableName + "_History");
        if (Helper.HasRows(ds)) grd.DataSource = ds.Tables[0];
        else grd.DataSource = null;
        grd.DataBind();
    }

    public void LoadData(int opt)
    {
        mltHistory.ActiveViewIndex = opt;
        string tableName = string.Empty;
        switch (opt)
        {
            case 1:
                LoadGrid("OWNER_XREF", grdOwnerRelationships);
                break;
            case 2:
                LoadGrid("OWNER_OTHER", grdOwnerOtherInfo);
                break;
            case 3:
                LoadGrid("OWNER_CONVICTION", grdConviction);
                break;
            case 4:
                LoadGrid("OWNER_DEBARRED", grdDebarred);
                break;
            case 5:
                LoadGrid("OWNER_EXCLUDED", grdExcluded);
                break;
            case 6:
                LoadGrid("OWNER_TERMINATED", grdTerminated);
                break;
            case 7:
                LoadGrid("OWNER_PENALTY", grdPenalty);
                break;
            case 8:
                LoadGrid("ORIGINAL_OWNER", grdOriginalOwner);
                break;
            case 9:
                LoadGrid("SUBCONTRACTOR", grdSubcontractor);
                break;
            case 10:
                LoadGrid("SUBCONTRACTOR_OWNER", grdSubcontractoOwner);
                break;
            case 11:
                LoadGrid("SUBCONTRACTOR5YRS", grdSubcontractor5Years);
                break;
            /*case 12:
                LoadGrid("SUPPLIER", grd);
                break;
            case 13:
                LoadGrid("SUPPLIER", grdSupplier);
                break;*/
        }
    }
}