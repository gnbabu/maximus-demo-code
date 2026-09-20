using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_CLIALabCodes : System.Web.UI.UserControl
{


    public string CLIANumber {
        get
        {
            return (string.IsNullOrEmpty(hdnCliaNumber.Value)) ? string.Empty : hdnCliaNumber.Value.ToString();
        }
        set
        {
            hdnCliaNumber.Value = value;
        }
    }

  
    public void LoadData()
    {
        BindCliaLabCodes();
    }
     private void BindCliaLabCodes()
    {
        try
        {
            DataSet dsLabcodes = new DataSet();
            if (!string.IsNullOrEmpty(CLIANumber))
            {

                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                dsLabcodes = svc.SelectCLIALabCodesByNumber(CLIANumber);

            }

            grdCLIALabCode.DataSource = dsLabcodes;
            grdCLIALabCode.DataBind();

        }
        catch (Exception ex)
        {

        }
    }

    protected void grdCLIALabCode_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist))
            {
                grdCLIALabCode.Columns[1].Visible = false;
            }
            else
            {
                grdCLIALabCode.Columns[1].Visible = true;
            }
        }
    }

    protected void grdCLIALabCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCLIALabCode.PageIndex = e.NewPageIndex;
       
        BindCliaLabCodes();
    }
}