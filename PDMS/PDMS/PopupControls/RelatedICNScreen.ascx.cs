using System;
using System.Data;

public partial class PopupControls_RelatedICNScreen : System.Web.UI.UserControl
{
    public string ICNNumber = string.Empty;
    public DataTable dtICN = new DataTable();
    public DataTable dsICN
    {
        get
        {
            if (!Helper.HasRows(dtICN))
            {
                if (Helper.HasRows(GetICN()))
                {
                    return dtICN;
                }
            }
            return dtICN;
        }
        set
        {
            if (value != null)
            {
                dtICN = value;
                SetICNs(dtICN);
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
        GetICN();
    }
    protected DataTable GetICN()
    {
        if (dtICN != null && dtICN.Rows.Count > 0)
        {
            gvRelatedICNs.DataSource = dtICN;
            gvRelatedICNs.DataBind();
        }
        return  dtICN;
    }
    private void SetICNs(DataTable dt)
    {
        gvRelatedICNs.DataSource = dt;
        gvRelatedICNs.DataBind();
    }
    public void ClearGrid()
    {
        gvRelatedICNs.DataSource = null;
        gvRelatedICNs.DataBind();
    }
}
