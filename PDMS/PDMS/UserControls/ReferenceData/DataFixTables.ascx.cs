using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_DataFixTables : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rgDataFixTables_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgDataFixTables.DataSource = psc.SelectDataFixTablesAll();
    }

    protected void rgDataFixTables_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        string tableName = valuesToUpdate["TABLE_NAME"] as string;
        string tableValue = valuesToUpdate["TABLE_VALUE"] as string;
        string tableType = valuesToUpdate["TABLE_TYPE"] as string;
        bool isVisible = (bool)valuesToUpdate["IS_VISIBLE"];
        string pkColumns = valuesToUpdate["PK_COLUMN"] as string;
        string columsHide = valuesToUpdate["COLUMNS_HIDE"] as string;
        string readonlyColumns = valuesToUpdate["READONLY_COLUMNS"] as string;
        DateTime lastmodifiedDate = DateTime.Now;
        Guid lastmodifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertDataFixTables(tableName, tableValue, tableType, isVisible, pkColumns, columsHide, readonlyColumns, lastmodifiedDate, lastmodifiedUser);
    }

    protected void rgDataFixTables_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        int id = int.Parse((gei).GetDataKeyValue("ID").ToString());
        string tableName = valuesToUpdate["TABLE_NAME"] as string;
        string tableValue = valuesToUpdate["TABLE_VALUE"] as string;
        string tableType = valuesToUpdate["TABLE_TYPE"] as string;
        bool isVisible = (bool)valuesToUpdate["IS_VISIBLE"];
        string pkColumns = valuesToUpdate["PK_COLUMN"] as string;
        string columsHide = valuesToUpdate["COLUMNS_HIDE"] as string;
        string readonlyColumns = valuesToUpdate["READONLY_COLUMNS"] as string;
        DateTime lastmodifiedDate = DateTime.Now;
        Guid lastmodifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateDataFixTables(id, tableName, tableValue, tableType, isVisible, pkColumns, columsHide, readonlyColumns, lastmodifiedDate, lastmodifiedUser);
    }

    protected void rgDataFixTables_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;

        int id = int.Parse((item).GetDataKeyValue("ID").ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.DeleteDataFixTable(id);

        // Rebind the grid
        rgDataFixTables.Rebind();
    }


}