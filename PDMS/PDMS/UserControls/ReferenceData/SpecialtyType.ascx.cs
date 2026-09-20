using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_SpecialtyType : System.Web.UI.UserControl
{

    protected void rgSpecialtyType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
      
        string specialtyTypeName = valuesToUpdate["SPECIALTY_TYPE_NAME"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string mmisSpecialtyTypeID = valuesToUpdate["MMIS_SPECIALTY_TYPE_ID"].ToString();
        string externalSpecialtyTypeName = valuesToUpdate["EXTERNAL_SPECIALTY_TYPE_NAME"].ToString();
        bool isVisible = (bool)valuesToUpdate["isVisible"];

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertSpecialtyType(specialtyTypeName, lastModifiedDate, lastModifiedUser, mmisSpecialtyTypeID, externalSpecialtyTypeName, isVisible);


    }
    protected void rgSpecialtyType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        int specialtyTypeID = int.Parse((gei).GetDataKeyValue("SPECIALTY_TYPE_ID").ToString());
        string specialtyTypeName = valuesToUpdate["SPECIALTY_TYPE_NAME"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string mmisSpecialtyTypeID = valuesToUpdate["MMIS_SPECIALTY_TYPE_ID"].ToString();
        string externalSpecialtyTypeName = valuesToUpdate["EXTERNAL_SPECIALTY_TYPE_NAME"].ToString();
        bool isVisible = (bool)valuesToUpdate["IsVisible"];

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateSpecialtyType(specialtyTypeID, specialtyTypeName, 
            lastModifiedDate, lastModifiedUser, mmisSpecialtyTypeID, externalSpecialtyTypeName, isVisible);
    }
    protected void rgSpecialtyType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgSpecialtyType.DataSource = psc.SelectAllSpecialtyTypesUnfiltered();
    }
}