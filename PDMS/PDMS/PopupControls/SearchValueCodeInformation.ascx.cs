using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SearchValueCodeInformation : System.Web.UI.UserControl
{
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }
            return _svc;
        }
    }
    #endregion

    #region Properties

    public string ValueCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnValueCode.Value))
                return hdnValueCode.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnValueCode.Value = value.Trim();
        }
    }
    public string ValueDescription
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnValueCodeDesc.Value))
                return hdnValueCodeDesc.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnValueCodeDesc.Value = value.Trim();
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

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.gvValueCodeSearchPop.CurrentPageIndex = 0;
           // RefreshData();
        }

    }

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
    protected void gvValueCodeSearchPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void gvValueCodeSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }

    protected void lnkValueCodeSpan_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        txtCode.Text = string.Empty;
        txtValueDesc.Text = string.Empty;
        hdnValueCode.Value = cmdArgument.ToString();
        hdnValueCodeDesc.Value = grdrow.Cells[1].Text;
    }

    public void RefreshData()
    {
        DataTable dt = GetData(gvValueCodeSearchPop.PageSize);
        if (Helper.HasRows(dt))
        {
            gvValueCodeSearchPop.DataSource = dt;
            gvValueCodeSearchPop.DataBind();
        }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sortColWithDirection = this.gvValueCodeSearchPop.GridViewSortDirection == SortDirection.Descending ? gvValueCodeSearchPop.GridViewSortColumn + " DESC" : gvValueCodeSearchPop.GridViewSortColumn;
        DataSet ds = GetValueCodeData();

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    private DataSet GetValueCodeData()
    {
        DataSet dsSpanCode;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("CLAIMS_VALUE_CODE", txtCode.Text);
        parms.Add("CLAIMS_VALUE_CODE_DESC", txtValueDesc.Text);
        dsSpanCode = svc.SelectPanelsData("CLAIMS_VALUE_CODE", parms);
        return dsSpanCode;
    }

    protected void btnSearch_Click2(object sender, EventArgs e)
    {
      
        RefreshData();
    }
}
