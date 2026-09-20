using AjaxControlToolkit;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_OwnerInfoHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public void LoadData()
    {
        mltOwnerHistory.ActiveViewIndex = 0;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER_History");
        if (Helper.HasRows(ds)) grdOwnerInfo.DataSource = ds.Tables[0];
        else grdOwnerInfo.DataSource = null;
        grdOwnerInfo.DataBind();
        
    }

    //OHPNM-15996-Adding Pagination to Owner History
    protected void GrdOwnerInfo_PageIndexChanging(object sender,GridViewPageEventArgs e)
    {
        //throw new System.NotImplementedException();
        grdOwnerInfo.PageIndex = e.NewPageIndex;
        LoadData();
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();

    }
}