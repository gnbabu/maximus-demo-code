using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_RegPageType : System.Web.UI.UserControl
{
    protected void rgRegPageType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgRegPageType.DataSource = psc.SelectRegPageTypeAll();
    }

    protected void rgRegPageType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);


        string regPageName = valuesToUpdate["REG_PAGE_NAME"] as string;

        int? sequenceID = null;
        if(!string.IsNullOrWhiteSpace(valuesToUpdate["SEQUENCE_ID"].ToString()))
            sequenceID = int.Parse(valuesToUpdate["SEQUENCE_ID"].ToString());

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertRegPageType(regPageName, lastModifiedDate, lastModifiedUser, sequenceID);
    }

    protected void rgRegPageType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        int regPageTypeID = int.Parse((gei).GetDataKeyValue("REG_PAGE_TYPE_ID").ToString());
        string regPageName = valuesToUpdate["REG_PAGE_NAME"] as string;

        int? sequenceID = null;
        if (!string.IsNullOrWhiteSpace(valuesToUpdate["SEQUENCE_ID"].ToString()))
            sequenceID = int.Parse(valuesToUpdate["SEQUENCE_ID"].ToString());

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateRegPageType(regPageTypeID, regPageName, lastModifiedDate, lastModifiedUser, sequenceID);
    }
}