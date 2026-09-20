using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_ProviderTypeFee : System.Web.UI.UserControl
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

    private DataSet GetProviderCategoryTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetProviderCategories(true);
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

    private DataSet providerTypes = null;
    private DataSet ProviderTypes
    {
        get
        {
            if (providerTypes == null)
                providerTypes = GetProviderTypes();
            return providerTypes;
        }
    }

    private DataSet GetProviderTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectAllProviderTypes();
    }

    protected void rgProviderTypeFee_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgProviderTypeFee.DataSource = psc.SelectProviderTypeFeeAll();
    }
    protected void rgProviderTypeFee_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddlProviderType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("ENTITY_TYPE_ID", ((gei).FindControl("ddlEntityType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);

        int providerTypeID = int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        bool isFeeRequired = (bool)valuesToUpdate["IS_FEE_REQUIRED"];
        decimal feeAmount = 0;
        if (valuesToUpdate["FEE_AMOUNT"] != null)
         feeAmount = decimal.Parse(valuesToUpdate["FEE_AMOUNT"].ToString());

        int entityTypeID = int.Parse(valuesToUpdate["ENTITY_TYPE_ID"].ToString());
        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);


       PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
       psc.InsertProviderTypeFee(providerTypeID, isFeeRequired, feeAmount, lastModifiedDate, lastModifiedUser, entityTypeID, applicationTypeID);

    }
    protected void rgProviderTypeFee_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddlProviderType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("ENTITY_TYPE_ID", ((gei).FindControl("ddlEntityType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);

        int providerTypeID = int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        bool isFeeRequired = (bool)valuesToUpdate["IS_FEE_REQUIRED"];
        decimal feeAmount = decimal.Parse(valuesToUpdate["FEE_AMOUNT"].ToString());
        int entityTypeID = int.Parse(valuesToUpdate["ENTITY_TYPE_ID"].ToString());
        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        int providerTypeFeeID = int.Parse((gei).GetDataKeyValue("PROVIDER_TYPE_FEE_ID").ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateProviderTypeFee(providerTypeFeeID, providerTypeID, isFeeRequired, feeAmount, lastModifiedDate, lastModifiedUser, entityTypeID, applicationTypeID);
    }
    protected void rgProviderTypeFee_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editForm = (GridEditFormItem)e.Item;
            DropDownList ddlEntityType = (DropDownList)editForm.FindControl("ddlEntityType");
            ddlEntityType.DataSource = ProviderCategoryTypes;
            ddlEntityType.DataValueField = "PROVIDER_CATEGORY_TYPE_ID";
            ddlEntityType.DataTextField = "PROVIDER_CATEGORY_TYPE_NAME";
            ddlEntityType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlEntityType.SelectedValue = ((DataRowView)e.Item.DataItem)["ENTITY_TYPE_ID"].ToString();
            DropDownList ddlProviderType = (DropDownList)editForm.FindControl("ddlProviderType");
            ddlProviderType.DataSource = ProviderTypes;
            ddlProviderType.DataValueField = "PROVIDER_TYPE_ID";
            ddlProviderType.DataTextField = "PROVIDER_TYPE_NAME";
            ddlProviderType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlProviderType.SelectedValue = ((DataRowView)e.Item.DataItem)["PROVIDER_TYPE_ID"].ToString();

            DropDownList ddlApplicationType = (DropDownList)editForm.FindControl("ddlApplicationType");
            ddlApplicationType.DataSource = ApplicationTypes;
            ddlApplicationType.DataValueField = "APPLICATION_TYPE_ID";
            ddlApplicationType.DataTextField = "APPLICATION_TYPE_NAME";
            ddlApplicationType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlApplicationType.SelectedValue = ((DataRowView)e.Item.DataItem)["APPLICATION_TYPE_ID"].ToString();
        }
    }
}