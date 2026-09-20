using System.Data;

public partial class PopupControls_EducationHistory : System.Web.UI.UserControl
{
    public void LoadData(DataTable dt)
    {
        grdEducationHistory.DataSource = dt;
        grdEducationHistory.DataBind();
    }
    protected string GetContactDetails(object name, object email, object phone)
    {
        string contact = string.Empty;
        string contactName = string.IsNullOrEmpty(name.ToString()) ? "" : name.ToString();
        string contactEmail = string.IsNullOrEmpty(email.ToString()) ? "" : email.ToString();
        string contactPhone = string.IsNullOrEmpty(phone.ToString()) ? "" : phone.ToString();
        contact = Helper.GetFormattedContact(contactName, contactEmail, contactPhone);

        return contact;
    }
    //public WorkflowPage WorkflowPage
    //{
    //    get { return (WorkflowPage)this.Page; }
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{

    //}

    //public override void LoadControlData()
    //{
    //}

    //public override void LoadData(System.Data.DataRow dr = null)
    //{
    //}

    //public override bool SaveData()
    //{
    //    return true;
    //}

    //public override bool ValidateData()
    //{
    //    return true;
    //}

    //public override string Title
    //{
    //    get { return "Work History"; }
    //}

    //public override string IdText
    //{
    //    get { return "ucWorkHistory_" + this.WorkflowPage.RegistrationId; }
    //}

    //public override string ValidationGroup
    //{
    //    get { return "vgWorkHistory"; }
    //}
}