using DocumentFormat.OpenXml.Drawing;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_PAAssignProcedureGrp : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private DataSet paAssignmentTypes = null;
    private DataSet paAssignmentProcCodes = null;
    private DataSet PAAssignmentTypes
    {
        get
        {
            if (paAssignmentTypes == null)
                paAssignmentTypes = GetPAAssignmentGroups();
            return paAssignmentTypes;
        }
    }

    private DataSet GetPAAssignmentGroups()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetPAAssignmentGroups();
    }

    private DataSet PAAssignmentProcCodes
    {
        get
        {
            if (paAssignmentProcCodes == null)
                paAssignmentProcCodes = GetPAAssignmentProcCodes();
            return paAssignmentProcCodes;
        }
    }

    private DataSet GetPAAssignmentProcCodes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetPAAssignmentProcCodes();
    }

    protected void rgPAProcedureGrp_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgPAProcedureGrp.DataSource = psc.SelectAllPAAssignProcedureGroups();
    }

    private DataSet GetClaimEffectiveDate(string procCode)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetClaimProcEffectiveDate(procCode);
    }

    protected void rgPAProcedureGrp_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("CDE_PA_ASSIGN", ((gei).FindControl("ddlPAAssignmentType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROC_FROM", ((gei).FindControl("ddlProcFrom") as DropDownList).SelectedItem.Value);
        //valuesToUpdate.Add("PROC_TO", ((gei).FindControl("txtProcTo") as TextBox).Text);
        //valuesToUpdate.Add("PROC_FROM_ORDER", ((gei).FindControl("txtProcFromOrder") as TextBox).Text);
        //valuesToUpdate.Add("PROC_TO_ORDER", ((gei).FindControl("txtProcToOrder") as TextBox).Text); 
        //valuesToUpdate.Add("DTE_EFFECTIVE", ((gei).FindControl("txtDateEffective") as TextBox).Text);
        //valuesToUpdate.Add("DTE_END", ((gei).FindControl("txtDateEnd") as TextBox).Text);

        string desc = (gei.FindControl("ddlPAAssignmentType") as DropDownList).SelectedItem.Text;
        string[] descArr = desc.Split(new char[] { '-' });
        string cdePAAssign = valuesToUpdate["CDE_PA_ASSIGN"] as string;
        string dsc50 = "OH PA Assignment " + cdePAAssign + "," + descArr[1];
        string procFrom = valuesToUpdate["PROC_FROM"] as string;
        string procTo = valuesToUpdate["PROC_TO"] as string;

        string procFromStr = (gei.FindControl("ddlProcFrom") as DropDownList).SelectedItem.Text;
        string[] procFromArr = procFromStr.Split(new char[] { '-' });
        string[] procFromOrderArr = procFromArr[1].Split(new char[] { ':' });
        int procFromOrder = Convert.ToInt32(procFromOrderArr[1]);

        //string procToStr = (gei.FindControl("txtProcTo") as Text).SelectedItem.Text;
        //string[] procToArr = procToStr.Split(new char[] { '-' });
        //string[] procToOrderArr = procToArr[1].Split(new char[] { ':' });
        int procToOrder = Convert.ToInt32(procFromOrderArr[1]);

        DateTime dteEffective = Convert.ToDateTime(valuesToUpdate["DTE_EFFECTIVE"]);
        DateTime dteEnd = Convert.ToDateTime(valuesToUpdate["DTE_END"]);
        DateTime lastModifiedDate = DateTime.Now;
        DateTime createdOnDate = DateTime.Now;
        string createdByUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();        

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertPAAssignProcedureGrp(cdePAAssign, dsc50, procFrom, procTo, procFromOrder, procToOrder, dteEffective, dteEnd, createdByUser, createdOnDate, lastModifiedDate);
    }

    protected void rgPAProcedureGrp_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("CDE_PA_ASSIGN", ((gei).FindControl("ddlPAAssignmentType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROC_FROM", ((gei).FindControl("ddlProcFrom") as DropDownList).SelectedItem.Value);
        //valuesToUpdate.Add("PROC_TO", ((gei).FindControl("ddlProcTo") as DropDownList).SelectedItem.Value);
        //valuesToUpdate.Add("PROC_FROM_ORDER", ((gei).FindControl("txtProcFromOrder") as TextBox).Text);
        //valuesToUpdate.Add("PROC_TO_ORDER", ((gei).FindControl("txtProcToOrder") as TextBox).Text);
        //valuesToUpdate.Add("DTE_EFFECTIVE", ((gei).FindControl("txtDateEffective") as TextBox).Text);
        //valuesToUpdate.Add("DTE_END", ((gei).FindControl("txtDateEnd") as TextBox).Text);

        int id = int.Parse((gei).GetDataKeyValue("ID").ToString());
        string desc = (gei.FindControl("ddlPAAssignmentType") as DropDownList).SelectedItem.Text;
        string[] descArr = desc.Split(new char[] { '-' });
        string cdePAAssign = valuesToUpdate["CDE_PA_ASSIGN"] as string;
        string dsc50 = "OH PA Assignment " + cdePAAssign + "," + descArr[1];
        string procFrom = valuesToUpdate["PROC_FROM"] as string;
        string procTo = valuesToUpdate["PROC_TO"] as string;

        string procFromStr = (gei.FindControl("ddlProcFrom") as DropDownList).SelectedItem.Text;
        string[] procFromArr = procFromStr.Split(new char[] { '-' });
        string[] procFromOrderArr = procFromArr[1].Split(new char[] { ':' });
        int procFromOrder = Convert.ToInt32(procFromOrderArr[1]);

        //string procToStr = (gei.FindControl("ddlProcTo") as DropDownList).SelectedItem.Text;
        //string[] procToArr = procToStr.Split(new char[] { '-' });
        //string[] procToOrderArr = procToArr[1].Split(new char[] { ':' });
        int procToOrder = Convert.ToInt32(procFromOrderArr[1]);

        DateTime dteEffective = Convert.ToDateTime(valuesToUpdate["DTE_EFFECTIVE"]);
        DateTime dteEnd = Convert.ToDateTime(valuesToUpdate["DTE_END"]);
        DateTime lastModifiedDate = DateTime.Now;
        DateTime createdOnDate = DateTime.Now;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdatePAAssignProcedureGrp(id, cdePAAssign, dsc50, procFrom, procTo, procFromOrder, procToOrder, dteEffective, dteEnd);
    }

    protected void rgPAProcedureGrp_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editform = (GridEditFormItem)e.Item;

            DropDownList ddlProviderCategoryType = (DropDownList)editform.FindControl("ddlPAAssignmentType");
            ddlProviderCategoryType.DataSource = PAAssignmentTypes;
            ddlProviderCategoryType.DataValueField = "PRIOR_AUTH_Assignment_Type_MMIS";
            ddlProviderCategoryType.DataTextField = "Desc1";
            ddlProviderCategoryType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlProviderCategoryType.SelectedValue = ((DataRowView)e.Item.DataItem)["CDE_PA_ASSIGN"].ToString();

            DropDownList ddlProcFromCode = (DropDownList)editform.FindControl("ddlProcFrom");
            ddlProcFromCode.DataSource = PAAssignmentProcCodes;
            ddlProcFromCode.DataValueField = "CLAIMS_HCPCS_PROCEDURE_CODE";
            ddlProcFromCode.DataTextField = "Claim_Code_Order";
            ddlProcFromCode.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlProcFromCode.SelectedValue = ((DataRowView)e.Item.DataItem)["PROC_FROM"].ToString();

            DropDownList ddlProcFrom = (DropDownList)editform.FindControl("ddlProcFrom");
            TextBox txtProcToCode = (TextBox)editform.FindControl("txtProcTo");
            TextBox txtDateEffective = (TextBox)editform.FindControl("txtDateEffective");
            TextBox txtDateEnd = (TextBox)editform.FindControl("txtDateEnd");
            HiddenField hdnProcTo = (HiddenField)editform.FindControl("hdnProcTo");
            HiddenField hdnDateEffective = (HiddenField)editform.FindControl("hdnDateEffective");
            HiddenField hdnDateEnd = (HiddenField)editform.FindControl("hdnDateEnd");

            if (ddlProcFrom != null)
            {
                var claimDS = GetClaimEffectiveDate(ddlProcFrom.SelectedValue);
                if (claimDS != null)
                {
                    string effDate = claimDS.Tables[0].Rows[0]["EFFECTIVE_DATE"].ToString() != "" ? Convert.ToDateTime(claimDS.Tables[0].Rows[0]["EFFECTIVE_DATE"].ToString()).ToShortDateString() : DateTime.Today.ToShortDateString();
                    string endDate = claimDS.Tables[0].Rows[0]["END_DATE"].ToString() != "" ? Convert.ToDateTime(claimDS.Tables[0].Rows[0]["END_DATE"].ToString()).ToShortDateString() : "12/31/2299";
                    
                    ddlProcFrom.Attributes.Add("onchange", "populateDates('" + ddlProcFrom.ClientID + "', '" + txtProcToCode.ClientID + "', '" + txtDateEffective.ClientID + "', '" + txtDateEnd.ClientID + "', '" 
                        + hdnProcTo.ClientID
                        + "', '"
                        + hdnDateEffective.ClientID
                        + "', '"
                        + hdnDateEnd.ClientID
                        + "', '"
                        + effDate
                        + "', '"
                        + endDate
                        + "'); ");
                }
            }

        }

        if (e.Item is GridFilteringItem)
        {
            GridFilteringItem eItem = (GridFilteringItem)e.Item;

        }
    }

    protected void rgPAProcedureGrp_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

}