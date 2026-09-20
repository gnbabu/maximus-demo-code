using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

public partial class Views_ProviderOperatorView : System.Web.UI.UserControl, IProviderOperatorView
{
    public delegate void NextInQueueEventtHandler(int paperRequestQueueID);
    public event NextInQueueEventtHandler NextInQueueEvent;

    public delegate void SelectMyProviderEventHandler(ProviderManagerData providerData);
    public event SelectMyProviderEventHandler SelectMyProviderEvent;
    
    public delegate void ErrorEventHandler(Dictionary<string, string> lstErrors);
    public event ErrorEventHandler ErrorEvent;
    
    private Guid UserID
    {
        get
        {
            return Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        }
    }

    private string TaxID
    {
        get;
        set;
    }

    public int PaperRequestTypeID
    {
        get
        {
            return ViewState["PaperRequestTypeID"] == null ? 0 : Convert.ToInt32(ViewState["PaperRequestTypeID"]);
        }
        set
        {
            ViewState["PaperRequestTypeID"] = value;
        }
    }
        

    private ProviderOperatorPresenter _presenter;

    public ProviderOperatorPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderOperatorPresenter(this);
            }

            return _presenter;
        }
    }
    
    public PaperRequestQueueData Model { get; set; }
    public void InitView()
    {
        if (Helper.IsModern())
        {
            tdMyDashGraph.Visible = true;
            tdMyDashGraphTable.Visible = true; 
        }
        else
        {
            tdMyDashGraph.Visible = false;
            tdMyDashGraphTable.Visible = false; 
        }

        this.TaxID = string.Empty;
        this.PaperRequestTypeID = 0;
        presenter.Init(UserID);
    }


    public void EnablePage(bool enable)
    {
        this.gvMyDashboard.Enabled = enable;
        this.gvMyProviders.Enabled = enable;
        if (enable && gvMyProviders.Rows.Count > 0 && this.gvMyProviders.SelectedIndex > -1)
        {
            this.gvMyProviders.SelectRow(-1);
        }
        this.btnNext.Enabled = enable;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        this.Model = new PaperRequestQueueData();
        Model.UserID = UserID;
        presenter.RequestNextPaperRequest();
    }


    protected void gvMyProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        Label lblStatus = e.Row.FindControl("lblStatus") as Label;
        LinkButton btn = e.Row.FindControl("btnManage") as LinkButton;
        if (lblStatus != null && btn != null)
        {
            if (lblStatus.Text == "Approved")
            {
                btn.Visible = false;
                lblStatus.Visible = true;
            }
            else
            {
                btn.Visible = true;
                lblStatus.Visible = false;
            }
        }

        string specialtyName = e.Row.Cells[5].Text;
        if (specialtyName.Length > 30)
        {
            e.Row.Cells[5].Text = specialtyName.Substring(0, 29);
        }
    }

    protected void gvMyProviders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();

    }

    protected void gvMyProviders_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();

    }
    protected void gvMyProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("SelectRow"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            gvMyProviders.SelectRow(index);

        }
    }

    protected void gvMyProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvMyProviders.SelectedIndex > -1)
        {
            if (SelectMyProviderEvent != null)
            {
                int paperQueueID = gvMyProviders.SelectedDataKey.Values["PaperRequestQueueID"].ToString() == string.Empty ? 0 : (int)gvMyProviders.SelectedDataKey.Values["PaperRequestQueueID"];
                int regID = gvMyProviders.SelectedDataKey.Values["RegID"].ToString() == string.Empty ? 0 : (int)gvMyProviders.SelectedDataKey.Values["RegID"];
                ProviderManagerData keyData = LoadKeyFields();
                keyData.RegID = regID;
                keyData.PaperRequestQueueID = paperQueueID;
                SelectMyProviderEvent(keyData);
            }
        }
    }

    protected void gvMyDashboard_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("SelectRow"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            gvMyDashboard.SelectRow(index);

        }
    }
    protected void gvMyDashboard_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (gvMyDashboard.SelectedIndex > -1)
        {
                PaperRequestTypeID = gvMyDashboard.SelectedDataKey.Values["PAPER_REQUEST_TYPE_ID"].ToString() == string.Empty ? 0 : (int)gvMyDashboard.SelectedDataKey.Values["PAPER_REQUEST_TYPE_ID"];
                RefreshMyProviders();
        }
    }
    private void InitModel()
    {
        if (Model == null)
        {
            presenter.Init();
            Model.UserID = this.UserID;
            Model.TaxID = this.TaxID;
            Model.PaperRequestTypeID = this.PaperRequestTypeID;
        }
    }

    private ProviderManagerData LoadKeyFields()
    {
        ProviderManagerData keyData = new ProviderManagerData();
        keyData.UserID = UserID;
        keyData.PaperRequestQueueID = PaperRequestTypeID;

        return keyData;
    }
    #region Presenter Events

    public void SetMyProviders(DataSet ds, int totalRowCount)
    {
        this.gvMyProviders.DataSource = ds;
        this.gvMyProviders.VirtualItemCount = totalRowCount;
        this.gvMyProviders.DataBind();
    }

    public void SetMyDashboard(DataSet ds)
    {
        this.gvMyDashboard.DataSource = ds;
        this.gvMyDashboard.DataBind();
    }

    public void SetNextInQueueNotFound()
    {
       //display a simple message
        this.lblInformationalMessage.Text = "No new paper requests waiting to be worked.";
    }

    public void SetNextInQueue(int paperRequestQueueID)
    {
        if (NextInQueueEvent != null)
        {
            NextInQueueEvent(paperRequestQueueID);
        }
    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            if (ErrorEvent != null)
                ErrorEvent(presenter.ErrorList);
        }
    }
    #endregion


    public void RefreshData()
    {
        this.ResetSelectedProvider();
        this.RefreshMyProviders();
    }

    private void RefreshMyProviders()
    {
        InitModel();
        string sortColWithDirection = this.gvMyProviders.GridViewSortDirection == SortDirection.Descending ? gvMyProviders.GridViewSortColumn + " DESC" : gvMyProviders.GridViewSortColumn;
        presenter.RequestMyProviderList(sortColWithDirection, gvMyProviders.PageSize, gvMyProviders.CurrentRowIndex, true);
    }

    private void ResetSelectedProvider()
    {
        if (this.gvMyProviders.SelectedIndex > -1)
            this.gvMyProviders.SelectedIndex = -1;
    }


}