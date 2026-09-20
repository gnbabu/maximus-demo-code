using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_ApplicationFeePaymentType : System.Web.UI.UserControl
{
    protected void rgApplicationFeePaymentType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgApplicationFeePaymentType.DataSource = psc.SelectApplicationFeePaymentTypeAll();
    }

    protected void rgApplicationFeePaymentType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        string paymentTypeName = valuesToUpdate["PAYMENT_TYPE_NAME"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertApplicationFeePaymentType(paymentTypeName, lastModifiedDate, lastModifiedUser);
    }

    protected void rgApplicationFeePaymentType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        int paymentTypeID = int.Parse((gei).GetDataKeyValue("PAYMENT_TYPE_ID").ToString());
        string paymentTypeName = valuesToUpdate["PAYMENT_TYPE_NAME"] as string;
        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateApplicationFeePaymentType(paymentTypeID, paymentTypeName, lastModifiedDate, lastModifiedUser);
    }
}