using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_CLIACertificationsInfo : System.Web.UI.UserControl
{
    public string CLIANumber
    {
        get
        {
            return (string.IsNullOrEmpty(hdnCliaNo.Value)) ? string.Empty : hdnCliaNo.Value.ToString();
        }
        set
        {
            hdnCliaNo.Value = value;
        }
    }

   
    public void LoadData()
    {
        BindCliaCertificationDataGrid();
    }

    private void BindCliaCertificationDataGrid()
    {
        try
        {
            DataSet dsCLIA = new DataSet();
            if (!string.IsNullOrEmpty(CLIANumber))
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                dsCLIA= svc.SelectCLIACertificateInfoByNumber(CLIANumber);

            }
            grdCLIACert.DataSource = dsCLIA;
            grdCLIACert.DataBind();
        }
        catch(Exception ex)
        {

        }
    }


    protected void grdCLIACert_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCLIACert.PageIndex = e.NewPageIndex;
        BindCliaCertificationDataGrid();
    }
}