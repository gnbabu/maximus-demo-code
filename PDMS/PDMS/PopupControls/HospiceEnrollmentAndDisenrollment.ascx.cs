using System;
using System.Linq;

public partial class PopupControls_HospiceEnrollmentAndDisenrollment : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public  string ElectionDate
    {
        get { return txtElectionDate.Text; }
    }
    public string DisenrollDate
    {
        get { return txtDisenrollment.Text; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.ElectionDisenrollDates != null)
        {
            txtElectionDate.Text = inquireResponse.Payload.ElectionDisenrollDates.ElectionDate.ToString("MM/dd/yyyy");
            txtDisenrollment.Text =Convert.ToDateTime( inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy");
        }
    }
}