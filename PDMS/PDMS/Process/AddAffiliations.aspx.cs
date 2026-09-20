using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

public partial class Process_AddAffiliations : WorkflowPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucGroupAffiliations.SaveDataEvent += ucGroupAffiliations_SaveDataEvent;
        //SetWorkflowPanel();
    }

    void ucGroupAffiliations_SaveDataEvent()
    {        // If Workflow does not exist add it
        List<SqlParameter> parametersForRegToLiveAffiliate = new List<SqlParameter>();
        parametersForRegToLiveAffiliate.Add(SqlParms.CreateParameter("GroupRegID", DbType.Int32, this.RegistrationId, true));

        DataSet ds = new DataSet();
        ds = DataAccess.ExecuteStoredProcedure("usp_TransferAffiliateRegistrationsToProviderTables", parametersForRegToLiveAffiliate, "NewProvider");
        //regtolive and create transaction
        List<SqlParameter> parameterParty = new List<SqlParameter>();

        parameterParty.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, this.RegistrationId, false));
        parameterParty.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
        parameterParty.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Helper.GetUserId(HttpContext.Current.User.Identity.Name), false));
        DataAccess.ExecuteStoredProcedure("usp_UpdateGroupAffiliations", parameterParty);
//        dirty.Value = "true";

        //regtolive affiliations
            Response.Redirect("~/Process/AddAffiliations.aspx?AllowEdit=true");
    }

    override protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ucGroupAffiliations.LoadData();
            Master.LoadProviderInfoHeader();
        }
        Master.SetWorkflowPanelVisibility(Master.ProcessID > 0);
        lblRegistrationId.Text = "<b>Registration Id:</b> " + this.RegistrationId.ToString();
    }

    public override bool OnTaskExit(string action)
    {
        // If the user clicks the "Submit" button then move to the next step and go back to Provider Home
        //btnCancelPage.Attributes.Add("onclick", "return confirmAndClean('Any changes you have made wi
        Master.RedirectURL = "~/Process/ProviderHomeNew.aspx";
        return true;
    }

}