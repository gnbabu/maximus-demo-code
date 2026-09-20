using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_ProviderType : System.Web.UI.UserControl
{
    private DataSet providerCategoryTypes = null;
    private DataSet ProviderCategoryTypes
    {
        get
        {
            if (providerCategoryTypes == null)
                providerCategoryTypes = GetProviderCategoryTypes();
            return providerCategoryTypes;
        }
    }

    private DataSet providerRiskLevels = null;
    private DataSet ProviderRiskLevels
    {
        get
        {
            if (providerRiskLevels == null)
                providerRiskLevels = GetProviderRiskLevels();
            return providerRiskLevels;
        }
    }

    private DataSet applicationTypes = null;
    private DataSet ApplicationTypes
    {
        get
        {
            if (applicationTypes == null)
                applicationTypes = GetApplicationTypes();
            return applicationTypes;
        }
    }
     

    private DataSet GetApplicationTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetApplicationTypes();
    }


    private DataSet GetProviderCategoryTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetProviderCategories(true);
    }

    DataSet GetProviderRiskLevels()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectProviderRiskLevels();
    }

     
    protected void rgProviderType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgProviderType.DataSource = psc.SelectAllProviderTypes();
    }

 
   

    protected void rgProviderType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("PROVIDER_CATEGORY_TYPE_ID", ((gei).FindControl("ddlProviderCategoryType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_RISK_LEVEL_ID", ((gei).FindControl("ddlProviderRiskLevel") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REQUIRE_NPI", ((gei).FindControl("chkNPIRequired") as CheckBox).Checked);
        valuesToUpdate.Add("IS_USED_IN_MMIS", ((gei).FindControl("chkIsUsedInMMIS") as CheckBox).Checked);

        string providerTypeAbbreviation = valuesToUpdate["PROVIDER_TYPE_ABBREVIATION"] as string;
        string providerTypeName = valuesToUpdate["PROVIDER_TYPE_NAME"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string isUsedInMMIS = (bool)valuesToUpdate["IS_USED_IN_MMIS"] == true ? "Y" : "N";
        int providerCategoryTypeID = int.Parse(valuesToUpdate["PROVIDER_CATEGORY_TYPE_ID"].ToString());
        string mmisProviderTypeID = valuesToUpdate["MMIS_PROVIDER_TYPE_ID"].ToString();
        bool npiRequired = (bool)valuesToUpdate["REQUIRE_NPI"];
        int providerRiskLevelID = int.Parse(valuesToUpdate["PROVIDER_RISK_LEVEL_ID"].ToString());
        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertProviderType(providerTypeAbbreviation, providerTypeName, lastModifiedDate, lastModifiedUser, isUsedInMMIS, providerCategoryTypeID, mmisProviderTypeID, npiRequired,
            providerRiskLevelID, applicationTypeID);
    }

    protected void rgProviderType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("PROVIDER_CATEGORY_TYPE_ID", ((gei).FindControl("ddlProviderCategoryType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_RISK_LEVEL_ID", ((gei).FindControl("ddlProviderRiskLevel") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REQUIRE_NPI", ((gei).FindControl("chkNPIRequired") as CheckBox).Checked);
        valuesToUpdate.Add("IS_USED_IN_MMIS", ((gei).FindControl("chkIsUsedInMMIS") as CheckBox).Checked);

        int providerTypeID = int.Parse((gei).GetDataKeyValue("PROVIDER_TYPE_ID").ToString());
        string providerTypeAbbreviation = valuesToUpdate["PROVIDER_TYPE_ABBREVIATION"] as string;
        string providerTypeName = valuesToUpdate["PROVIDER_TYPE_NAME"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string isUsedInMMIS = (bool)valuesToUpdate["IS_USED_IN_MMIS"] == true ? "Y" : "N";
        int providerCategoryTypeID = int.Parse(valuesToUpdate["PROVIDER_CATEGORY_TYPE_ID"].ToString());
        string mmisProviderTypeID = valuesToUpdate["MMIS_PROVIDER_TYPE_ID"].ToString();
        bool npiRequired = (bool)valuesToUpdate["REQUIRE_NPI"];
        int providerRiskLevelID = int.Parse(valuesToUpdate["PROVIDER_RISK_LEVEL_ID"].ToString());
        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateProviderType(providerTypeID, providerTypeAbbreviation, providerTypeName, lastModifiedDate, lastModifiedUser, isUsedInMMIS, providerCategoryTypeID, mmisProviderTypeID, npiRequired,
            providerRiskLevelID, applicationTypeID);
    }

    protected void rgProviderType_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editform = (GridEditFormItem)e.Item;

            DropDownList ddlProviderRiskLevel = (DropDownList)editform.FindControl("ddlProviderRiskLevel");
            ddlProviderRiskLevel.DataSource = ProviderRiskLevels;
            ddlProviderRiskLevel.DataValueField = "PROVIDER_RISK_LEVEL_ID";
            ddlProviderRiskLevel.DataTextField = "PROVIDER_RISK_LEVEL_NAME";
            ddlProviderRiskLevel.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlProviderRiskLevel.SelectedValue = ((DataRowView)e.Item.DataItem)["PROVIDER_RISK_LEVEL_ID"].ToString();


            DropDownList ddlApplicationType = (DropDownList)editform.FindControl("ddlApplicationType");
            ddlApplicationType.DataSource = ApplicationTypes;
            ddlApplicationType.DataValueField = "APPLICATION_TYPE_ID";
            ddlApplicationType.DataTextField = "APPLICATION_TYPE_NAME";
            ddlApplicationType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlApplicationType.SelectedValue = ((DataRowView)e.Item.DataItem)["APPLICATION_TYPE_ID"].ToString();


            DropDownList ddlProviderCategoryType = (DropDownList)editform.FindControl("ddlProviderCategoryType");
            ddlProviderCategoryType.DataSource = ProviderCategoryTypes;
            ddlProviderCategoryType.DataValueField = "PROVIDER_CATEGORY_TYPE_ID";
            ddlProviderCategoryType.DataTextField = "PROVIDER_CATEGORY_TYPE_NAME";
            ddlProviderCategoryType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlProviderCategoryType.SelectedValue = ((DataRowView)e.Item.DataItem)["PROVIDER_CATEGORY_TYPE_ID"].ToString();
        }

        if (e.Item is GridFilteringItem)
        {
            GridFilteringItem eItem = (GridFilteringItem)e.Item;

            RadComboBox combo = (RadComboBox)eItem.FindControl("RadComboBoxRisk");
            combo.DataSource = null;
            combo.Items.Clear();
            DataTable dt = ProviderRiskLevels.Tables[0].DefaultView.ToTable(true, new String[] { "Provider_Risk_Level_Name" });
            DataRow dr = dt.NewRow();
            dr["Provider_Risk_Level_Name"] = "All";
            dt.Rows.InsertAt(dr, 0);
            combo.DataSource = dt;
            combo.DataBind();

            combo.SelectedValue = ((GridItem)e.Item).OwnerTableView.GetColumn("ProviderRiskLevelName").CurrentFilterValue;


            RadComboBox comboAppType = (RadComboBox)eItem.FindControl("RadComboBoxApplication");
            comboAppType.DataSource = null;
            comboAppType.Items.Clear();
            DataTable dt1 = ApplicationTypes.Tables[0].DefaultView.ToTable(true, new String[] { "Application_Type_Name" }); ;
            DataRow dr1 = dt1.NewRow();
            dr1["Application_Type_Name"] = "All";
            dt1.Rows.InsertAt(dr1, 0);
            comboAppType.DataSource = dt1;
            comboAppType.DataBind();

            comboAppType.SelectedValue = ((GridItem)e.Item).OwnerTableView.GetColumn("ApplicationTypeName").CurrentFilterValue;

            RadComboBox RadMMIS = (RadComboBox)eItem.FindControl("RadMMIS");
            RadMMIS.SelectedValue = ((GridItem)e.Item).OwnerTableView.GetColumn("UsedInMMIS").CurrentFilterValue;


        }
    }

    protected void rgProviderType_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
    }
}