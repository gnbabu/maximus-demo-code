public partial class PopupControls_SpecialityFocus : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public string CertificateSpecialty {
        get {
           return  tbSpecialty.Text;
        }
        set {
            tbSpecialty.Text = value;
                }
    }

    public string Focus
    {
        get { return tbFocus.Text; }
        set { tbFocus.Text = value; }
    }
    public string CertifyingOrganization
    {
        get { return tbCertifyingOrg.Text; }
        set { tbCertifyingOrg.Text = value; }
    }

    public string CertificationDate
    {
        get { return tbCertificationDate.Text; }
        set { tbCertificationDate.Text = value; }
    }
    public string CertificationExpirationDate
    {
        get { return tbCertificationEndDate.Text; }
        set { tbCertificationEndDate.Text = value; }
    }
    public string EndorsementStatus
    {
        get { return tbEndorsemnetStatus.Text; }
        set { tbEndorsemnetStatus.Text = value; }
    }
    public string EndorsementNumber
    {
        get { return tbEndorsemnetNumber.Text; }
        set { tbEndorsemnetNumber.Text = value; }
    }

    public void SetReadonly(bool value) {

        tbEndorsemnetNumber.ReadOnly = tbFocus.ReadOnly = tbSpecialty.ReadOnly = tbCertifyingOrg.ReadOnly=tbEndorsemnetStatus.ReadOnly= value;
        tbCertificationDate.Enabled = tbCertificationEndDate.Enabled = CalendarExtenderDate.Enabled = CalendarExtenderEndDate.Enabled = !value;
        if (value)
        {
            tbEndorsemnetNumber.BackColor = tbFocus.BackColor=tbSpecialty.BackColor=tbCertifyingOrg.BackColor= tbCertificationDate.BackColor = tbCertificationEndDate.BackColor = tbEndorsemnetStatus.BackColor= System.Drawing.Color.LightGray;
        }
        else
        {
            tbEndorsemnetNumber.BackColor = tbFocus.BackColor = tbSpecialty.BackColor = tbCertifyingOrg.BackColor = tbCertificationDate.BackColor= tbCertificationEndDate.BackColor= tbEndorsemnetStatus.BackColor=  System.Drawing.Color.White;
        }
    }
       
    public void LoadValues(string focus,string specialty, string org,string status,string certificationdate,string certificationenddate, string endorsemnetNumber)
    {
        tbEndorsemnetNumber.Text = endorsemnetNumber;
        tbFocus.Text = focus;
        tbSpecialty.Text = specialty;
        tbCertifyingOrg.Text = org;
        tbCertificationDate.Text = certificationdate;
        tbCertificationEndDate.Text = certificationenddate;
        tbEndorsemnetStatus.Text = status;
    }
    public void Visible(bool value)
    {
        pnlSpecialtyFocus.Visible = value;
    }
   

    public  string Title
    {
        get;set;
        
    }

    //public override string IdText
    //{
    //    get { return "ucSpecialtyFocus_" + this.WorkflowPage.RegistrationId; }
    //}
    //public override void LoadControlData()
    //{
        
    //}
    //public override bool ValidateData()
    //{
    //    return true;
    //}
    //public override bool SaveData()
    //{
    //    return true;
    //}
    //public override string ValidationGroup
    //{
    //    //As there is no required section/field in this part setting it to empty, need to revisit later
    //    get { return string.Empty; }
    //}
    //public override void LoadData(DataRow row)
    //{
    //}
    //    protected void Page_Load(object sender, EventArgs e)
    //{
                                                               
    //}
}