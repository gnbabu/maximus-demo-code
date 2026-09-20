using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimOccurrenceSpanInformation : BasePopupControl
{
    #region spa
    private PDMSService.PDMSServiceClient _spa;


    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion
    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
                //txtProviderNotes.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

    }


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
    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();



    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _spa = new PDMSService.PDMSServiceClient();
        this.ValidateData();
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }



        try
        {
           
            return;
        }
        catch (Exception ex)
        {

        }

    }
    private void ValidateData()
    {

    }

    //protected void ReportOccurenceDate_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    if (Helper.IsValidDate(txtOccurenceDate.Text, true) && Helper.IsValidDate(txtOccurenceDate.Text, true))
    //    {
    //        TimeSpan ts = Convert.ToDateTime(txtOccurenceDate.Text).Subtract(Convert.ToDateTime(txtOccurenceDate.Text));
    //        args.IsValid = (ts.Days < 366 && ts.Days > -366);
    //    }
    //    else
    //        args.IsValid = true;

    //    if (!args.IsValid)
    //    {

    //    }
    //}
    protected void ReportFromDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtoccFromDate.Text, true) && Helper.IsValidDate(txtoccFromDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtoccFromDate.Text).Subtract(Convert.ToDateTime(txtoccFromDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }

    protected void ReportToDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtoccToDate.Text, true) && Helper.IsValidDate(txtoccToDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtoccToDate.Text).Subtract(Convert.ToDateTime(txtoccToDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void lnkOccurenceSpanInfo_Click(object sender, EventArgs e)
    {

    }
    

 protected void gvOccurenceSpanInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


       // BindGrid();
    }
    protected void OccurrenceSpanInformationAdd_Click(object sender, EventArgs e)
    {
        ValidateData();


    }


    protected void gvOccurenceSpanInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOccurenceSpanInfo.PageIndex = e.NewPageIndex;
       // BindGrid();
        gvOccurenceSpanInfo.EditIndex = -1;
    }

    protected void lnkConditionCodeSearch_Click(object sender, EventArgs e)
    {

    }

    protected void lnkValueCodeSearch_Click(object sender, EventArgs e)
    {

    }

    public void ClearGrid()
    {
        gvOccurenceSpanInfo.DataSource = null;
        gvOccurenceSpanInfo.DataBind();
    }
}