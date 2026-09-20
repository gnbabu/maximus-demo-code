using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_CARCRARCINformation : System.Web.UI.UserControl
{
    public DataTable dtCARC= new DataTable();
    public string ICNNumber = string.Empty;
    public DataTable ds
    {
        get
        {
            if (!Helper.HasRows(dtCARC))   
            {
                if (Helper.HasRows(GetCARC()))
                {
                    return dtCARC;
                }
            }
            return dtCARC;
        }
        set
        {
            if (value != null)
            {
                dtCARC = value;
                SetCARC(dtCARC);
            }
        }
    }

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
        GetCARC();
    }
    protected DataTable GetCARC()
    {
        if (dtCARC != null && dtCARC.Rows.Count > 0)
        {
            gvCARCandRARCDetails.DataSource = dtCARC;
            gvCARCandRARCDetails.DataBind();
        }
        return dtCARC;
    }
    private void SetCARC(DataTable dt)
    {
        gvCARCandRARCDetails.DataSource = dt;
        gvCARCandRARCDetails.DataBind();
    }

    public void ClearGrid()
    {
        gvCARCandRARCDetails.DataSource = null;
        gvCARCandRARCDetails.DataBind();
    }
}