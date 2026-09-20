using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_ClaimsXtenInformation : System.Web.UI.UserControl
{
    public DataTable dtXten = new DataTable();
    public DataTable dtCXten
    {
        get
        {
            if (!Helper.HasRows(dtXten))
            {
                if (Helper.HasRows(GetXtenInfo()))
                {
                    return dtXten;
                }
            }
            return dtXten;
        }
        set
        {
            if (value != null)
            {
                dtXten = value;
                SetXtenInfo(dtXten);
            }
        }
    }

    public string ICNNumber = string.Empty;
    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ICNNumber))
                return ICNNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ICNNumber = value.Trim();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        GetXtenInfo();
    }
    protected DataTable GetXtenInfo()
    {
        if (dtXten != null && dtXten.Rows.Count > 0)
        {
            gvClaimXtenInfo.DataSource = dtXten;
            gvClaimXtenInfo.DataBind();
        }
        return dtXten;        
    }

    private void SetXtenInfo(DataTable dt)
    {
        gvClaimXtenInfo.DataSource = dt;
        gvClaimXtenInfo.DataBind();
    }
    public void ClearGrid()
    {
        gvClaimXtenInfo.DataSource = null;
        gvClaimXtenInfo.DataBind();
    }
}