using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

public partial class PopupControls_HospiceDocumentsByMail_ : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
        LoadHospiceDocumentTypeDropDown();
    }

    private void BindGrid()
    {
        if (!string.IsNullOrEmpty(this.WorkflowPage.HospiceTrackNo))
        {
            var ds = RegistrationController.SelectHospiceDocumentByMail(this.WorkflowPage.HospiceTrackNo);

            gvHospiceDocumentsByMail.DataSource = ds.Tables[0];
            gvHospiceDocumentsByMail.DataBind();
        }
    }

    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        var docType = ddlDocumentType.SelectedValue;
        var note = txtNote.Text.Trim();
        /// <summary>
        /// This Method will Save Document By mail
        /// </summary>
        Dictionary<string, string> parms = new Dictionary<string, string>();

        parms.Add("HOSPICE_TRACKING_NUMBER", this.WorkflowPage.HospiceTrackNo);
        var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        parms.Add("HOSPICE_DOCUMENT_TYPE_ID", docType);
        parms.Add("HOSPICE_DOCUMENT_MAIL_DESC", note);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", id.ToString());
        parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
        parms.Add("Created_By_User", id.ToString());
        RegistrationController.InsertHospiceDocumentByMail(parms);

        BindGrid();
    }
    protected void gvHospiceDocumentsByMail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceDocumentsByMail.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceDocumentsByMail.EditIndex = -1;
    }

    protected void gvHospiceDocumentsByMail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // fetch UserId from DataKeys
        int hospiceDocumentsByMailId = Convert.ToInt32(gvHospiceDocumentsByMail.DataKeys[e.RowIndex]["HospiceDocumentsByMailId"].ToString());

        //Delete in db
        BindGrid();
    }
    private void LoadHospiceDocumentTypeDropDown()
    {
        Helper.LoadDropDown(ddlDocumentType, GetSelectHospiceDocumentType().Tables[0], "DOCUMENT_TYPE_DESC", "DOCUMENT_TYPE_CODE", true);

    }
    private DataSet GetSelectHospiceDocumentType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospiceDocumentType();
        }
    }
}