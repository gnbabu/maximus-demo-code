using System;
using System.Web.UI;

public partial class CustomException : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var lastError = Session["LastError"] as System.Exception;

            if (lastError == null)
            {
                if (Request["Message"] != null)
                {
                    lblMessage.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["Message"].ToString(), true);
                }
                else
                {
                    string errorPath = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request.QueryString["aspxerrorpath"], true);
                    if (string.IsNullOrEmpty(errorPath))
                    {
                        lblMessage.Text = "Unknown error.";
                    }
                    else if (errorPath.Contains("NoSuchPage.aspx"))
                    {
                        lblMessage.Text = "Page not found.";
                    }
                    else
                    {
                        lblMessage.Text = errorPath;
                    }
                }
            }
            else
            {
                lblMessage.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(lastError.Message, true);
                lblErrorType.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(lastError.GetType().ToString(), true);
                lblStackTrace.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(lastError.StackTrace, true);
            }
        }
        catch (System.Exception ex)
        {
            lblMessage.Text = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(ex.Message, true);
        }

        this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName") + " - Error";
        RenderPage.Render("Resources", phAutoGenerate);
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        if (pnlDetails.Visible)
        {
            pnlDetails.Visible = false;
            btnShowDetail.Text = "Show Details";
        }
        else
        {
            pnlDetails.Visible = true;
            btnShowDetail.Text = "Hide Details";
        }
    }
}