using Amazon.Runtime.Internal.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ReferenceData_AutomatedReports : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rgAutomatedReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgAutomatedReports.DataSource = psc.SelectAllAutomatedReports();
    }

    protected void rgAutomatedReports_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
            GridEditableItem gei = (GridEditableItem)e.Item;
            e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("REPORT_NAME", valuesToUpdate["REPORT_NAME"] as string);
            parms.Add("REPORT_ID", new Guid(valuesToUpdate["REPORT_ID"].ToString()));
            parms.Add("REPORT_SUBJECT", valuesToUpdate["REPORT_SUBJECT"] as string);
            parms.Add("REPORT_FROM", valuesToUpdate["REPORT_FROM"] as string);
            parms.Add("REPORT_TO", valuesToUpdate["REPORT_TO"] as string);
            parms.Add("REPORT_BCC", valuesToUpdate["REPORT_BCC"] as string);
            parms.Add("REPORT_SQL", valuesToUpdate["REPORT_SQL"] as string);
            parms.Add("REPORT_TEMPLATE", valuesToUpdate["REPORT_TEMPLATE"] as string);
            parms.Add("REPORT_ATTACHMENT", valuesToUpdate["REPORT_ATTACHMENT"] as string);
            parms.Add("CRON_EXP", valuesToUpdate["CRON_EXP"] as string);
            parms.Add("ACTIVE", (bool)valuesToUpdate["ACTIVE"]);
            parms.Add("ENVIRONMENT", valuesToUpdate["ENVIRONMENT"] as string);
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name));
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name));
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
            parms.Add("OPS", CON.AutomatedReports.AutomationReportMainInsert);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.InsertUpdateAutomatedReportsMain(parms);

            AddSuccess(CON.AutomatedReports.AutomatedReportsMainUISuccessInsert);
        }
        catch (Exception ex)
        {
            AddError(string.Format("Exception : {0} {1}",ex.Message, ex.StackTrace));
        }
    }

    protected void rgAutomatedReports_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
            GridEditableItem gei = (GridEditableItem)e.Item;
            e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("REPORT_ID", valuesToUpdate["REPORT_ID"] as string);
            parms.Add("OPS", CON.AutomatedReports.AutomationReportMainDelete);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.InsertUpdateAutomatedReportsMain(parms);

            AddSuccess(CON.AutomatedReports.AutomatedReportsMainUISuccessDelete);
        }
        catch (Exception ex)
        {
            AddError(string.Format("Exception : {0} {1}", ex.Message, ex.StackTrace));
        }
    }

    protected void btnGridResults_Click(object sender, EventArgs e)
    {
        try
        {
            Guid gu = new Guid();
            bool isValid = Guid.TryParse(txtReportIDGrid.Text, out gu);
            if (isValid)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectAutomatedReportsSub(gu);
                if (Helper.HasRows(ds)) grdAutomatedReportSub.DataSource = ds.Tables[0];
                else grdAutomatedReportSub.DataSource = null;
                grdAutomatedReportSub.DataBind();
            }
            else
            {
                AddError(CON.AutomatedReports.AutomatedReportsMainUIInvalidGuid);
            }
        }
        catch (Exception ex)
        {
            AddError(string.Format("Exception : {0} {1}", ex.Message, ex.StackTrace));
        }
    }

    protected void btnDeleteGrid_Click(object sender, EventArgs e)
    {
        try
        {
            Guid gu = new Guid();
            bool isValid = Guid.TryParse(txtReportIDGrid.Text, out gu);
            if (isValid)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("REPORT_ID", txtReportIDGrid.Text);
                parms.Add("OPS", CON.AutomatedReports.AutomationReportSubDelete);
                psc.InsertUpdateAutomatedReportsMain(parms);
                grdAutomatedReportSub.DataSource = null;
                grdAutomatedReportSub.DataBind();
            }
            else
            {
                AddError(CON.AutomatedReports.AutomatedReportsMainUIInvalidGuid);
            }
        }
        catch (Exception ex)
        {
            AddError(string.Format("Exception : {0} {1}", ex.Message, ex.StackTrace));
        }
    }

    protected void rgAutomatedReports_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
            GridEditableItem gei = (GridEditableItem)e.Item;
            e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("ID", valuesToUpdate["ID"] as string);
            parms.Add("REPORT_NAME", valuesToUpdate["REPORT_NAME"] as string);
            parms.Add("REPORT_ID", new Guid(valuesToUpdate["REPORT_ID"].ToString()));
            parms.Add("REPORT_SUBJECT", valuesToUpdate["REPORT_SUBJECT"] as string);
            parms.Add("REPORT_FROM", valuesToUpdate["REPORT_FROM"] as string);
            parms.Add("REPORT_TO", valuesToUpdate["REPORT_TO"] as string);
            parms.Add("REPORT_BCC", valuesToUpdate["REPORT_BCC"] as string);
            parms.Add("REPORT_SQL", valuesToUpdate["REPORT_SQL"] as string);
            parms.Add("REPORT_TEMPLATE", valuesToUpdate["REPORT_TEMPLATE"] as string);
            parms.Add("REPORT_ATTACHMENT", valuesToUpdate["REPORT_ATTACHMENT"] as string);
            parms.Add("CRON_EXP", valuesToUpdate["CRON_EXP"] as string);
            parms.Add("ACTIVE", (bool)valuesToUpdate["ACTIVE"]);
            parms.Add("ENVIRONMENT", valuesToUpdate["ENVIRONMENT"] as string);
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name));
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name));
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
            parms.Add("OPS", CON.AutomatedReports.AutomationReportMainUpdate);


            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.InsertUpdateAutomatedReportsMain(parms);

            AddSuccess(CON.AutomatedReports.AutomatedReportsMainUISuccessUpdate);
        }
        catch (Exception ex)
        {
            AddError(string.Format("Exception : {0} {1}", ex.Message, ex.StackTrace));
        }
    }


    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "ARValidationSummaryGP";
        this.Page.Validators.Add(val);
    }
    private void AddSuccess(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "ARValidationSummarySuccessGP";
        this.Page.Validators.Add(val);
    }

}