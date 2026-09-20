using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_ProviderCategoryType : System.Web.UI.UserControl
{

    protected void rgProviderCategoryType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgProviderCategoryType.DataSource = psc.GetProviderCategories(true);
    }
    protected void rgProviderCategoryType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        string providerCategoryTypeName = valuesToUpdate["PROVIDER_CATEGORY_TYPE_NAME"] as string;
        bool isActivePhase2 = (bool)valuesToUpdate["IsActive_PHASEII"];
        string mmisProviderCategoryTypeID = valuesToUpdate["MMIS_PROVIDER_CATEGORY_TYPE_ID"] as string;
        string imageSource = valuesToUpdate["IMAGE_SRC"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        psc.InsertProviderCategoryType(providerCategoryTypeName,lastModifiedDate, lastModifiedUser, isActivePhase2, mmisProviderCategoryTypeID, imageSource);
    }
    protected void rgProviderCategoryType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        int providerCategoryTypeID = int.Parse((gei).GetDataKeyValue("PROVIDER_CATEGORY_TYPE_ID").ToString());
        string providerCategoryTypeName = valuesToUpdate["PROVIDER_CATEGORY_TYPE_NAME"] as string;
        bool isActivePhase2 = (bool)valuesToUpdate["IsActive_PHASEII"];
        string mmisProviderCategoryTypeID = valuesToUpdate["MMIS_PROVIDER_CATEGORY_TYPE_ID"] as string;
        string imageSource = valuesToUpdate["IMAGE_SRC"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        psc.UpdateProviderCategoryType(providerCategoryTypeID, providerCategoryTypeName, lastModifiedDate, lastModifiedUser, isActivePhase2, mmisProviderCategoryTypeID, imageSource);
    }
}