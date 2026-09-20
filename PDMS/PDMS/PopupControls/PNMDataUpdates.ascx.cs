using ClosedXML.Excel;
using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MAXIMUS.Core.Libraries;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PNMDataUpdates : System.Web.UI.UserControl
{
    public delegate void RegistrationViewEventHandler(int registrationId);
    public event RegistrationViewEventHandler RegistrationViewEvent;

    #region Properties
    private bool UserCanRapidAdminRegistrations
    {
        get
        {
            return Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name);
        }
    }

    private int SelectedRegID
    {
        get
        {
            return ViewState["SelectedRegID"] == null ? 0 : Convert.ToInt32(ViewState["SelectedRegID"]);
        }
        set
        {
            ViewState["SelectedRegID"] = value;
        }
    }


    private string RegOwnerTypeId
    {
        get
        {
            return ViewState["RegOwnerTypeId"] == null ? string.Empty : ViewState["RegOwnerTypeId"].ToString();
        }
        set
        {
            ViewState["RegOwnerTypeId"] = value;
        }
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.txtProcessID.Visible = false;
            this.txtTaskID.Visible = false;
            this.lblProcessID.Visible = false;
            this.lblTaskID.Visible = false;
            this.txtEndDate.Visible = false;
            this.lblEndDate.Visible = false;

            SetActiveView(actionDropDown.SelectedValue);
        }

        if (actionDropDown.Items.Count <= 1)
        {
            populateActionDropDown(0, "");
        }

        SetVersionControlFieldsVisibility(false);
    }

    private void GetLookupDataUpdateTrace()
    {
        lblApprovalStatus.Text = string.Empty;
        lblApprovalDate.Text = string.Empty;
        lblReviewDate.Text = string.Empty;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();        
        DataSet dsLookupTraceData = svc.GetLookupDataUpdateTrace(tableDropdownId.SelectedValue);
        if (dsLookupTraceData != null && dsLookupTraceData.Tables.Count > 0 && Helper.HasRows((dsLookupTraceData.Tables[0])))
        {
            SetVersionControlFieldsVisibility(true);

            lblApprovalStatus.Text = dsLookupTraceData.Tables[0].Rows[0]["APPROVAL_STATUS"].ToString();
            lblApprovalDate.Text = dsLookupTraceData.Tables[0].Rows[0]["APPROVED_DATE"].ToString();
            lblReviewDate.Text = dsLookupTraceData.Tables[0].Rows[0]["REVIEWED_DATE"].ToString();
        }
        else
        {
            SetVersionControlFieldsVisibility(false);
        }

        // svc.SaveLookupDataUpdateTrace("","","");
    }

    private void SetVersionControlFieldsVisibility(bool visible)
    {
        this.lblApprovalDate.Visible = visible;
        this.lblReviewDate.Visible = visible;
        this.lblApprovalStatus.Visible = visible;

        this.lblApprovalDateLabel.Visible = visible;
        this.lblReviewDateLabel.Visible = visible;
        this.lblApprovalStatusLabel.Visible = visible;
    }

    private DataTable GetDynamicData(out int totalResultCount, int pageSize)
    {
        int REGID = 0;
        if (txtRegId.Text != "")
        {
            bool isInt32 = Int32.TryParse(txtRegId.Text, out REGID);
        }
        if (this.tableDropdownId.SelectedValue == "")
        {
            totalResultCount = 0;
            return new DataTable();
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds, ds2, ds3;
        string primaryKeyCol = "";
        totalResultCount = 0;
        int pagenumber = ViewState["DynamicGridData"] != null ? int.Parse(ViewState["DynamicGridData"].ToString()) : 1;
        ds2 = psc.GetDataFixTableSpecificDetails(tableDropdownId.SelectedValue);
        DataTable dt = ds2.Tables[0];

        string tabletype = dt.Rows[0]["TABLE_TYPE"].ToString();
        ds = psc.SearchByREGID(REGID.ToString(), this.tableDropdownId.SelectedValue, tabletype, pageSize, pagenumber, this.txtMedId.Text);

        ds3 = psc.GetDataFixTablesDetails(this.tableDropdownId.SelectedValue);
        if (ds3 != null && Helper.HasRows(ds3))
        {
            primaryKeyCol = ds3.Tables[0].Rows[0]["PK_COLUMN"].ToString();
        }

        //Remove hidden columns from the display
        var hdnCols = dt.Rows[0]["COLUMNS_HIDE"].ToString().Split(',');

        foreach (string col in hdnCols)
        {
            if (ds.Tables[0].Columns.Contains(col))
            {
                if (primaryKeyCol.ToLower() != col.ToLower())
                {
                    ds.Tables[0].Columns.Remove(col);
                }
            }
        }

        if (Helper.HasRows(ds))
        {
            if (ds.Tables[1].Rows.Count >= 1)
            {
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    totalResultCount = (int)item["Count"];
                }
            }
            //Check the specific non-char fields we need to edit and add specific code for those to convert to varchar
            if (tableDropdownId.SelectedValue == "REG_OWNER")
            {
                string oldColumnName = "PERCENTAGE_OF_OWNERSHIP";
                string newColumnName = oldColumnName + "PERCENTAGE_OF_OWNERSHIP_STR";

                // 1. Add new string column
                ds.Tables[0].Columns.Add(newColumnName, typeof(string));

                // 2. Copy and convert data
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row[oldColumnName] != DBNull.Value)
                        row[newColumnName] = row[oldColumnName].ToString();
                    else
                        row[newColumnName] = DBNull.Value;
                }

                // 3. (Optional) Set the new column's position to match the old column
                int ordinal = ds.Tables[0].Columns[oldColumnName].Ordinal;
                ds.Tables[0].Columns[newColumnName].SetOrdinal(ordinal);

                // 4. Remove the old decimal column
                ds.Tables[0].Columns.Remove(oldColumnName);

                // 5. Rename the new column to the original name
                ds.Tables[0].Columns[newColumnName].ColumnName = oldColumnName;
            }

            return ds.Tables[0];
        }
        //else return new DataTable();
        else
        {
            if (this.actionDropDown.SelectedValue == "Update_Data" && ds.Tables[0].Rows.Count < 1)
            {
                DataRow newBlankRow = ds.Tables[0].NewRow(); //OHPNM-15112 Adding empty row for Insert functionality
                ds.Tables[0].Rows.InsertAt(newBlankRow, ds.Tables[0].Rows.Count);
                totalResultCount = 1;
                return ds.Tables[0];
            }
            else
                return new DataTable();
        }
    }

    protected void DynamicGrid1_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        LoadDataForDynamicGrid1();
    }

    protected void lnkExcel_Click(object sender, ImageClickEventArgs e)
    {
        int totalResultCount = 0;


        DynamicGrid1.DataSource = GetDynamicData(out totalResultCount, 10000);
        DynamicGrid1.DataBind();
        string alternateText = (sender as ImageButton).AlternateText;
        DynamicGrid1.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), alternateText);
        DynamicGrid1.ExportSettings.IgnorePaging = true;
        DynamicGrid1.ExportSettings.ExportOnlyData = true;
        DynamicGrid1.ExportSettings.OpenInNewWindow = true;
        DynamicGrid1.ExportSettings.FileName = "Data_Fix";
        DynamicGrid1.MasterTableView.ExportToExcel();

    }


    protected void DynamicGrid1_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        ViewState["DynamicGridData"] = e.NewPageIndex.ToString();
        LoadDataForDynamicGrid1();
    }

    protected void DynamicGrid1_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        LoadDataForDynamicGrid1();
    }

    protected void DynamicGrid1_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }


    protected void DynamicGrid1_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try {
            if (e.CommandName == RadGrid.UpdateCommandName)
            {
                var editableItem = e.Item as GridEditableItem;
                var editableNPI = editableItem.OwnerTableView.Columns.FindByUniqueNameSafe("NPI") != null ? editableItem["NPI"].Text : null;
                Hashtable hashTable = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(hashTable, editableItem);
                string userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

                if (e.Item is GridEditableItem)
                {
                    if (tableDropdownId.SelectedItem.Text.ToLower() == "update provider type")
                    {
                        ViewState["EditedItemHashTable"] = hashTable;
                        ViewState["EditedItemNPI"] = editableNPI;
                        ViewState["EditedItemUserId"] = userId;
                        ViewState["InsertAction"] = 0;

                        mpeConfirmActions.Show();
                    }
                    else
                    {
                        DynamicGridUpdateData(editableNPI, hashTable, userId);
                    }
                }
            }            
        } catch (Exception ex)
        {
            AddError(ex.Message + " " + ex.StackTrace.ToString(), "DataFix");
        }
    }

    private bool DynamicGridUpdateData(string editableNPI, Hashtable hashTable, string userId, string recordStatus = "A")
    {
        bool isError = false;
        if (hashTable != null && hashTable.Count > 0)
        {
            if (this.tableDropdownId.SelectedValue.Length > 0)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();
                DataSet dataSet = _spa.GetTableColumnsByTableName(tableDropdownId.SelectedValue, CON.TableOperations.Update);
                DataTable dtdatafixtable = dataSet.Tables[0];
                string columnName = "";
                string datatype = "";
                string isNotNullable = "";
                string isPopulated = "NO";
                string remainingNull = "";

                foreach (DataRow dftbs in dataSet.Tables[0].Rows)
                {
                    columnName = dftbs["COLUMN_NAME"].ToString();
                    datatype = dftbs["DATA_TYPE"].ToString();
                    isNotNullable = dftbs["IS_NULLABLE"].ToString();
                    isPopulated = "NO";

                    //if (datatype.Equals("int") && (columnName.Equals("REG_ID") || columnName.Equals("REGID")))
                    //{
                    //    if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                    //    {
                    //        parameters.Add(SqlParms.CreateParameter(columnName, DbType.Int32, Convert.ToInt32(hashTable[columnName]), true));
                    //        isPopulated = "YES";
                    //    }
                    //}

                    if (datatype.Equals("int") && (columnName.Equals("REG_ID") || columnName.Equals("REGID")))
                    {
                        if (!string.IsNullOrEmpty(txtRegId.Text))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Int32, Convert.ToInt32(txtRegId.Text), true));
                            isPopulated = "YES";
                        }
                    }
                    else if (datatype.Equals("int") && !(columnName.Equals("REG_ID") || columnName.Equals("REGID")))
                    {
                        if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Int32, Convert.ToInt32(hashTable[columnName]), true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("bit"))
                    {
                        if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Boolean, (bool)hashTable[columnName], true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("time"))
                    {
                        if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Time, hashTable[columnName], true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("date"))
                    {
                        if (columnName.Equals("ADR_END_DATE"))
                        {
                            if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                            {
                                parameters.Add(SqlParms.CreateParameter(columnName, DbType.DateTime, hashTable[columnName], true));
                                isPopulated = "YES";
                            }
                        }

                    }
                    if (datatype.Equals("datetime"))
                    {
                        if (columnName.Equals("Created_On_Date_Time") || columnName.Equals("CREATE_DATE_TIME") || columnName.Equals("CREATED_ON_DATE_TIME") || columnName.Equals("LAST_MODIFIED_DATE_TIME"))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.DateTime, DateTime.Now, true));
                            isPopulated = "YES";
                        }
                        else
                        {
                            if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                            {
                                parameters.Add(SqlParms.CreateParameter(columnName, DbType.DateTime, hashTable[columnName].ToString(), true));
                                isPopulated = "YES";
                            }
                        }
                    }
                    if (datatype.Equals("decimal"))
                    {
                        if (tableDropdownId.SelectedValue == "REG_OWNER")
                        {
                            if (columnName.Equals("PERCENTAGE_OF_OWNERSHIP"))
                            {
                                parameters.Add(SqlParms.CreateParameter(columnName, DbType.Decimal, Convert.ToDecimal(hashTable[columnName].ToString()), true));
                                isPopulated = "YES";
                            }
                        }
                    }
                    if (datatype.Equals("varchar") || datatype.Equals("nvarchar"))
                    {
                        if (hashTable.ContainsKey(columnName) && (hashTable[columnName] != null || editableNPI == "&nbsp;"))
                        {
                            string editableNPIvalue = hashTable[columnName] != null ? hashTable[columnName].ToString() : (editableNPI == "&nbsp;" ? "" : editableNPI);
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.String, editableNPIvalue, true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("uniqueidentifier"))
                    {
                        if (columnName.Equals("Created_By_User") || columnName.Equals("CREATED_BY_USER") || columnName.Equals("CREATED_BY") || columnName.Equals("LAST_MODIFIED_USER"))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Guid, userId, true));
                            isPopulated = "YES";
                        }
                        else
                        {
                            if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                            {
                                parameters.Add(SqlParms.CreateParameter(columnName, DbType.Guid, hashTable[columnName], true));
                                isPopulated = "YES";
                            }
                        }
                    }

                    if (tableDropdownId.SelectedItem.Text.ToLower() == "update provider type" && columnName.ToLower() == "record_status")
                    {
                        if (hashTable.ContainsKey(columnName))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Guid, recordStatus, true));
                            isPopulated = "YES";
                        }
                    }

                    if (isPopulated.Equals("NO") && isNotNullable.Equals("NO"))
                    {
                        remainingNull = remainingNull + " | " + columnName;
                    }
                }
                if (string.IsNullOrEmpty(remainingNull))
                {
                    DataAccess.ExecuteStoredProcedure("usp_pnmupdate" + this.tableDropdownId.SelectedValue, parameters);
                    SaveProviderFeed("Update Data on Table - " + this.tableDropdownId.SelectedValue, userId);

                    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                    svc.SaveLookupDataUpdateTrace(tableDropdownId.SelectedValue, recordStatus, userId);
                }
                else
                {
                    isError = true;
                    AddError("Remaining Fields to be populated : " + remainingNull, "DataFix");
                }
            }
        }

        LoadDataForDynamicGrid1();

        return isError;
    }

    protected void DynamicGrid1_CancelCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        LoadDataForDynamicGrid1();
    }

    protected void DynamicGrid1_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
    {
        var dataBoundColumn = e.Column as GridBoundColumn;


        if (dataBoundColumn != null)
        {
            PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();
            DataSet dataSet = _spa.GetDataFixTablesDetails(CON.DataFixTypes.REG);
            DataTable dtdatafixtable = dataSet.Tables[0];
            string readonlyColumns = "";
            string hideColumns = "";

            foreach (DataRow dftbs in dataSet.Tables[0].Rows)
            {
                if (tableDropdownId.SelectedValue == dftbs["TABLE_NAME"].ToString())
                {
                    readonlyColumns = dftbs["READONLY_COLUMNS"].ToString();
                    string[] valuesReadOnly = readonlyColumns.Split(',');
                    for (int i = 0; i < valuesReadOnly.Length; i++)
                    {
                        if (dataBoundColumn.DataField == valuesReadOnly[i].Trim())
                        {
                            dataBoundColumn.ReadOnly = true;
                        }
                    }

                    hideColumns = dftbs["COLUMNS_HIDE"].ToString();
                    string[] valueshide = hideColumns.Split(',');
                    for (int i = 0; i < valueshide.Length; i++)
                    {
                        if (dataBoundColumn.DataField == valueshide[i].Trim())
                        {
                            int i2 = 0;
                            if (DynamicGrid1.MasterTableView.Columns.Contains(valueshide[i].Trim()))
                            {
                                i2 = DynamicGrid1.MasterTableView.Columns.IndexOf(valueshide[i].Trim());
                                DynamicGrid1.MasterTableView.Columns[i2].Display = false;
                            }

                        }
                    }
                }
                if (dataBoundColumn.IsEditable &&
                    (dataBoundColumn.DataField == "HospitalNo" || dataBoundColumn.DataField == "NursingFacility" || dataBoundColumn.DataField == "HEALTH_CENTER_NO"))
                {
                    dataBoundColumn.MaxLength = 4;
                }
            }
        }

        var isDatePicker = e.Column as GridDateTimeColumn;
        if (isDatePicker != null)
        {
            isDatePicker.MaxDate = new DateTime(9999, 12, 31);
        }


    }

    protected void DynamicGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        //var dataItem = e.Item as GridDataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            var tableView = dataItem.OwnerTableView;
            var gridColumn = tableView.Columns.FindByUniqueNameSafe("RowNumber");
            if (gridColumn != null)
            {
                string firstRowNumberValue = dataItem["RowNumber"].Text;
                if (dataItem.ItemIndex == 0 && e.Item.ItemType.ToString() == "Item" && (firstRowNumberValue == "&nbsp;" || string.IsNullOrEmpty(firstRowNumberValue)))
                {
                    dataItem.Visible = false;
                    dataItem.Display = false;
                }
            }
        }
    }

    protected void btnApplyNow_Click(object sender, EventArgs e)
    {
        bool isError = false;
        Hashtable hashTable = ViewState["EditedItemHashTable"] as Hashtable;
        string NPI = ViewState["EditedItemNPI"] !=null? ViewState["EditedItemNPI"].ToString() : string.Empty;
        string userId = ViewState["EditedItemUserId"] != null ? ViewState["EditedItemUserId"].ToString() : string.Empty;
        int isInsertAction = ViewState["InsertAction"] != null ? Convert.ToInt32(ViewState["InsertAction"]) : 0;
        if (hashTable != null)
        {
            if (isInsertAction == 0)
            {
                isError = DynamicGridUpdateData(NPI, hashTable, userId, "A");
               
            }
            else
            {
                isError = DynamicGridInsertData(hashTable, userId, "A");
            }
        }
        GetLookupDataUpdateTrace();

        mpeConfirmActions.Hide();
    }

    protected void btnQueueProcessing_Click(object sender, EventArgs e)
    {
        bool isError = false;
        Hashtable hashTable = ViewState["EditedItemHashTable"] as Hashtable;
        string NPI = ViewState["EditedItemNPI"] != null ? ViewState["EditedItemNPI"].ToString() : string.Empty;
        string userId = ViewState["EditedItemUserId"] != null ? ViewState["EditedItemUserId"].ToString() : string.Empty;
        int isInsertAction = ViewState["InsertAction"] != null ? Convert.ToInt32(ViewState["InsertAction"]) : 0;
        if (hashTable != null)
        {
            if (isInsertAction == 0)
            {
                isError = DynamicGridUpdateData(NPI, hashTable, userId, "P");
            }
            else
            {
                isError = DynamicGridInsertData(hashTable, userId, "P");
            }
        }
        GetLookupDataUpdateTrace();

        mpeConfirmActions.Hide();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpeConfirmActions.Hide();
    }    

    private void LoadDataForDynamicGrid1()
    {
        int totalResultCount = 0;
        DynamicGrid1.Visible = true;
        DynamicGrid1.DataSource = GetDynamicData(out totalResultCount, 10000);
        if (totalResultCount > 0)
        {
            lnkExcel.Visible = true;
            //DynamicGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            if (this.actionDropDown.SelectedValue == "Update_Data")
                DynamicGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            else
                DynamicGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        }
        else
        {
            lnkExcel.Visible = false;
            DynamicGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        }
        DynamicGrid1.DataBind();
        DynamicGrid1.VirtualItemCount = totalResultCount;
    }

    protected void table_dropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.tableDropdownId.SelectedValue == null || this.tableDropdownId.SelectedValue.Length == 0)
        {
            return;
        }

        ViewState["DynamicGridData"] = null;
        LoadDataForDynamicGrid1();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["DynamicGridData"] = null;
        int regId = string.IsNullOrEmpty(txtRegId.Text) ? 0 : Convert.ToInt32(txtRegId.Text);
        string notes = string.Empty;
        if (this.actionDropDown.SelectedValue == "Update_Data")
        {
            if (this.tableDropdownId.SelectedValue == null || this.tableDropdownId.SelectedValue.Length == 0)
            {
                return;
            }
            //GetLookupDataUpdateTrace();
            LoadDataForDynamicGrid1();

            //Add notes only when the records data is changed - OHPNM-20119
            //notes = this.actionDropDown.SelectedItem.Text + " - " + this.tableDropdownId.SelectedItem.Text;
            //ProviderFeedHelper.InsertProviderFeedNotes(regId, 0, HttpContext.Current.User.Identity.Name, notes, enrollmentType: Constants.EnrollmentType.DataFix);

        }
        else
        {
            this.lblApprovalDate.Visible = false;
            this.lblReviewDate.Visible = false;
            this.lblApprovalStatus.Visible = false;

            this.lblApprovalDateLabel.Visible = false;
            this.lblReviewDateLabel.Visible = false;
            this.lblApprovalStatusLabel.Visible = false;

            notes = this.actionDropDown.SelectedItem.ToString();
            string userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, txtRegId.Text, true));
            parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, txtMedId.Text, true));

            if (this.actionDropDown.SelectedValue == "Cancel_Workflow")
            {
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_Workflow_Cancel", parameters);
            }
            else if (this.actionDropDown.SelectedValue == "Send_Update_Transaction")
            {

                parameters.Add(SqlParms.CreateParameter("TransactionTypeID", DbType.Int32, 1, true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.String, userId, true));
                parameters.Add(SqlParms.CreateParameter("SendHistory", DbType.Boolean, chkSendHistory.Checked, true));

                DataAccess.ExecuteStoredProcedure("usp_Send_Update_Transaction", parameters);
            }
            else if (this.actionDropDown.SelectedValue == "Return_to_Screening")
            {
                parameters.Add(SqlParms.CreateParameter("TaskName", DbType.String, "Initiate Provider Screening", true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.String, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_Move_WorkFlow_To_ProviderStep", parameters);
            }
            else if (this.actionDropDown.SelectedValue == "Return_to_Credentialing")
            {
                parameters.Add(SqlParms.CreateParameter("TaskName", DbType.String, "Initiate Provider Credentialing", true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.String, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_Move_WorkFlow_To_ProviderStep", parameters);
            }
            else if (this.actionDropDown.SelectedValue == "Move_Workflow") //Move Workflow to Next Step
            {
                notes = notes + " - Process ID - " + txtProcessID.Text + ", Task ID - " + txtTaskID.Text;
                parameters.Clear();
                parameters.Add(SqlParms.CreateParameter("PROCESSID", DbType.Int32, txtProcessID.Text, true));
                parameters.Add(SqlParms.CreateParameter("TASKID", DbType.Int32, txtTaskID.Text, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, txtRegId.Text, true));
                parameters.Add(SqlParms.CreateParameter("USER", DbType.String, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_Move_WorkFlow_To_NextStep", parameters);
            }
            else if (this.actionDropDown.SelectedValue == "Reopen_Not_Processed_Application")
            {
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_Reopen_NotProcessed_Application", parameters);
            }
            else if (this.actionDropDown.SelectedValue == "Change_End_Date") //Move Workflow to Next Step
            {
                notes = notes + " - End Date - " + txtEndDate.Text;
                parameters.Clear();
                if (string.IsNullOrEmpty(txtRegId.Text))
                {
                    parameters.Add(SqlParms.CreateParameter("REGID", DbType.Int32, 0, true));
                }
                else
                {
                    parameters.Add(SqlParms.CreateParameter("REGID", DbType.Int32, txtRegId.Text, true));
                }
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, txtMedId.Text, true));
                parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, txtEndDate.Text, true));
                parameters.Add(SqlParms.CreateParameter("loggedinuser", DbType.Guid, Helper.GetUserId(HttpContext.Current.User.Identity.Name), true));
                DataAccess.ExecuteStoredProcedure("usp_pnmupdate_end_dates", parameters);

                txtRegId.Text = "";
                txtEndDate.Visible = false;
                lblEndDate.Visible = false;
            }
            else if (this.actionDropDown.SelectedValue == "Reactivate_Provider")
            {
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userId, true));
                DataAccess.ExecuteStoredProcedure("usp_pnm_ReactivateProvider", parameters);
                txtRegId.Text = "";
                actionDropDown.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(notes))
            {
                ProviderFeedHelper.InsertProviderFeedNotes(regId, 0, HttpContext.Current.User.Identity.Name, notes, enrollmentType: Constants.EnrollmentType.DataFix);
            }
        }

    }

    private void SaveProviderFeed(string notes, string userId)
    {
        
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(SqlParms.CreateParameter("Reg_ID", DbType.Int32, txtRegId.Text, true));
            parms.Add(SqlParms.CreateParameter("Notes_Date", DbType.DateTime, DateTime.Now, true));
            parms.Add(SqlParms.CreateParameter("Initiated_By", DbType.Guid, userId, true));
            parms.Add(SqlParms.CreateParameter("Person_Reviewed_By", DbType.Guid, userId, true));
            parms.Add(SqlParms.CreateParameter("Enrollment_Type", DbType.String, "Data Fix Tab", true));
            parms.Add(SqlParms.CreateParameter("Final_Disposition", DbType.String, "Other", true));
            parms.Add(SqlParms.CreateParameter("NOTES", DbType.String, notes, true));
            parms.Add(SqlParms.CreateParameter("Last_Modified_User", DbType.Guid, userId, true));
            parms.Add(SqlParms.CreateParameter("Last_Modified_Date_time", DbType.DateTime, DateTime.Now, true));
            parms.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, userId, true));
            parms.Add(SqlParms.CreateParameter("CreatedOn", DbType.DateTime, DateTime.Now, true));

            try
            {
                DataAccess.ExecuteStoredProcedure("insertreg_provider_feed", parms);
            }
            catch (Exception ex)
            {

            }
            parms.Clear();
        }

    protected void actionDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        tableDropdownId.Items.Clear();
        tableDropdownId.Items.Add(new ListItem("Select a Table", ""));
        if (this.actionDropDown.SelectedValue == "Update_Data")
        {
            PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();
            DataSet dataSet = _spa.GetDataFixTablesDetails(CON.DataFixTypes.REG);
            DataTable dt = dataSet.Tables[0];

            this.btnSearch.Enabled = true;
            this.tableDropDownLableID.Visible = true;
            this.tableDropdownId.Visible = true;
            this.btnSave.Visible = true;
            Helper.LoadList(tableDropdownId, dt, "TABLE_VALUE", "TABLE_NAME", true);

            this.txtProcessID.Visible = false;
            this.txtTaskID.Visible = false;
            this.lblProcessID.Visible = false;
            this.lblTaskID.Visible = false;
            this.lblEndDate.Visible = false;
            this.txtEndDate.Visible = false;
            this.chkSendHistory.Visible = false;
        }
        else if (this.actionDropDown.SelectedValue == "Move_Workflow")
        {
            this.btnSearch.Enabled = true;
            this.btnSearch.Visible = true;
            this.tableDropDownLableID.Visible = false;
            this.tableDropdownId.Visible = false;
            this.txtProcessID.Visible = true;
            this.txtTaskID.Visible = true;
            this.lblProcessID.Visible = true;
            this.lblTaskID.Visible = true;
            this.btnSave.Visible = false;
            this.DynamicGrid1.Visible = false;
            this.lblEndDate.Visible = false;
            this.txtEndDate.Visible = false;
            this.chkSendHistory.Visible = false;
        }
        else if (this.actionDropDown.SelectedValue == "")
        {
            this.btnSearch.Enabled = true;
            this.tableDropDownLableID.Visible = true;
            this.tableDropdownId.Visible = true;
            this.btnSave.Visible = false;
            //this.DynamicGrid1.Visible = true;

            this.txtProcessID.Visible = false;
            this.txtTaskID.Visible = false;
            this.lblProcessID.Visible = false;
            this.lblTaskID.Visible = false;
            this.lblEndDate.Visible = false;
            this.txtEndDate.Visible = false;
            this.chkSendHistory.Visible = false;
        }
        else if (this.actionDropDown.SelectedValue == "Change_End_Date")
        {
            this.btnSearch.Enabled = true;
            this.tableDropDownLableID.Visible = false;
            this.tableDropdownId.Visible = false;
            this.btnSave.Visible = false;
            //this.DynamicGrid1.Visible = true;

            this.txtProcessID.Visible = false;
            this.txtTaskID.Visible = false;
            this.lblProcessID.Visible = false;
            this.lblTaskID.Visible = false;
            this.lblEndDate.Visible = true;
            this.txtEndDate.Visible = true;
            this.chkSendHistory.Visible = false;
        }
        else if (this.actionDropDown.SelectedValue == "Send_Update_Transaction")
        {
            ViewState["DynamicGridData"] = null;
            this.tableDropDownLableID.Visible = false;
            this.tableDropdownId.Visible = false;
            this.btnSave.Visible = false;
            this.DynamicGrid1.DataSource = new DataTable();
            this.DynamicGrid1.DataBind();
            this.DynamicGrid1.Visible = false;
            this.btnSearch.Enabled = true;

            this.txtProcessID.Visible = false;
            this.txtTaskID.Visible = false;
            this.lblProcessID.Visible = false;
            this.lblTaskID.Visible = false;
            this.lblEndDate.Visible = false;
            this.txtEndDate.Visible = false;
            this.chkSendHistory.Visible = true;
        }
        else if (this.actionDropDown.SelectedValue == "ReProcess_Delegate_Affiliate_File")
        {
            ViewState["DynamicGridData"] = null;
            this.tableDropDownLableID.Visible = false;
            this.tableDropdownId.Visible = false;
            this.btnSave.Visible = false;
            this.DynamicGrid1.DataSource = new DataTable();
            this.DynamicGrid1.DataBind();
            this.DynamicGrid1.Visible = false;
            this.btnSearch.Enabled = false;
            this.btnSearch.Visible = false;

            this.txtProcessID.Visible = false;
            this.txtTaskID.Visible = false;
            this.lblProcessID.Visible = false;
            this.lblTaskID.Visible = false;
            this.lblEndDate.Visible = false;
            this.txtEndDate.Visible = false;
            this.chkSendHistory.Visible = false;
            SetActiveView(actionDropDown.SelectedValue);
        }
        else
        {
            ViewState["DynamicGridData"] = null;
            this.tableDropDownLableID.Visible = false;
            this.tableDropdownId.Visible = false;
            this.btnSave.Visible = false;
            this.DynamicGrid1.DataSource = new DataTable();
            this.DynamicGrid1.DataBind();
            this.DynamicGrid1.Visible = false;
            this.btnSearch.Enabled = true;

            this.txtProcessID.Visible = false;
            this.txtTaskID.Visible = false;
            this.lblProcessID.Visible = false;
            this.lblTaskID.Visible = false;
            this.lblEndDate.Visible = false;
            this.txtEndDate.Visible = false;
            this.chkSendHistory.Visible = false;
        }

    }

    protected void tableDropdownId_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.actionDropDown.SelectedValue == "Update_Data")
        {
            if (this.tableDropdownId.SelectedValue == null || this.tableDropdownId.SelectedValue.Length == 0)
            {
                return;
            }
            GetLookupDataUpdateTrace();            
        }
        else
        {
            this.lblApprovalDate.Visible = false;
            this.lblReviewDate.Visible = false;
            this.lblApprovalStatus.Visible = false;
        }
    }

    private void SetActiveView(string viewName)
    {
        switch (viewName)
        {
            case "ReProcess_Delegate_Affiliate_File":
                mltDataFixes.SetActiveView(vwDelegateAffiliations);
                break;
            default:
                mltDataFixes.SetActiveView(View3);
                break;
        }
    }

    protected void txtRegId_TextChanged(object sender, EventArgs e)
    {
        //actionDropDown.Items.Clear();
        //actionDropDown.Items.Add(new ListItem("Select an Action", ""));
        //if ((this.txtRegId != null && this.txtRegId.Text.Length > 0) || (this.txtMedId != null && this.txtMedId.Text.Length > 0))
        //{
        //    int intt = 0;
        //    bool isInt = Int32.TryParse(txtRegId.Text, out intt);

        //    populateActionDropDown(intt, "");
        //}
        //DynamicGrid1.Visible = false;
        //lnkExcel.Visible = false;
    }

    protected void txtMedId_TextChanged(object sender, EventArgs e)
    {
        actionDropDown.Items.Clear();
        actionDropDown.Items.Add(new ListItem("Select an Action", ""));
        if ((this.txtMedId != null && this.txtMedId.Text.Length > 0) || (this.txtRegId != null && this.txtRegId.Text.Length > 0))
        {
            populateActionDropDown(0,txtMedId.Text);            
        }
        if (!string.IsNullOrWhiteSpace(this.txtMedId.Text.Trim()))
        {
            try
            {
                GetRegIdByMedId(this.txtMedId.Text.Trim());
            }
            catch
            { 

            }
        }
        DynamicGrid1.Visible = false;
        lnkExcel.Visible = false;
    }

    private void GetRegIdByMedId(string Medid)
    {
        PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();
        DataSet dataSet = _spa.GetRegIdByMedId(this.txtMedId.Text.Trim());
        DataTable dt = dataSet.Tables[0];
        this.txtRegId.Text = dt.Rows[0]["reg_id"].ToString();
    }

    private void populateActionDropDown(int regid = 0, string medicaid_id = "")
    {

        PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();

        actionDropDown.Items.Clear();
        DataSet dataSet = _spa.GetDataFixActions(regid = 0, medicaid_id = "");
        DataTable dt = dataSet.Tables[0];

        string EnabledDataFixChangeEndDate = AppSettings.Get("EnableDataFixChangeEndDate");
        if (EnabledDataFixChangeEndDate == "false")
        {
            foreach (DataRow row in dt.Select("DATA_FIX_ACTIONS_CODE = 'Change_End_Date'"))
            {
                row.Delete();
            }
            dt.AcceptChanges();
            //actionDropDown.Items.Remove(actionDropDown.Items.FindByValue("Change_End_Date"));
        }

        Helper.LoadList(actionDropDown, dt, "data_fix_actions_description", "data_fix_actions_code", true);


        tableDropdownId.Items.Clear();
        tableDropdownId.Items.Add(new ListItem("Select a Table", ""));
    }

    protected void DynamicGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

        if (e.CommandName == RadGrid.DeleteCommandName)
        {

            if (e.Item is GridItem)
            {
                var editableItem = e.Item as GridEditableItem;
                Hashtable hashTable = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(hashTable, editableItem);

                if (hashTable != null && hashTable.Count > 0)
                {
                    if (this.tableDropdownId.SelectedValue.Length > 0)
                    {

                        PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();
                        DataSet dataSet = _spa.GetDataFixTableSpecificDetails(tableDropdownId.SelectedValue);
                        DataTable dt = dataSet.Tables[0];
                        string pkColumn = "";
                        string pkDataType = "";

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            pkColumn = dt.Rows[0]["PK_COLUMN"].ToString();
                            pkDataType = dt.Rows[0]["PK_DATA_TYPE"].ToString();
                        }

                        List<SqlParameter> parameters = new List<SqlParameter>();

                        if (pkDataType.Equals("int"))
                        {
                            parameters.Add(SqlParms.CreateParameter(pkColumn, DbType.Int32, Convert.ToInt32(hashTable[pkColumn]), true));
                        }
                        else if(pkDataType.Equals("uniqueidentifier"))
                        {
                            parameters.Add(SqlParms.CreateParameter(pkColumn, DbType.Guid, hashTable[pkColumn], true));
                        }
                        else
                        {
                            parameters.Add(SqlParms.CreateParameter(pkColumn, DbType.String, hashTable[pkColumn].ToString(), true));
                        }

                        DataAccess.ExecuteStoredProcedure("usp_pnmDelete" + tableDropdownId.SelectedValue, parameters);
                        
                        string notes = "Delete Data on table - " + tableDropdownId.SelectedValue + " for id value - " + Convert.ToInt32(hashTable[pkColumn]).ToString();
                        SaveProviderFeed(notes, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    }

                }
            }

            LoadDataForDynamicGrid1();
            }
        }
        catch(Exception ex)
        {
            AddError(ex.Message +" "+ex.StackTrace.ToString(), "DataFix");
        }
    }
    protected void DynamicGrid1_InsertCommand(object sender, GridCommandEventArgs e)
    {
        int i = DynamicGrid1.Items.Count;
        GridDataItem item = e.Item as GridDataItem;
        if (tableDropdownId.SelectedValue == "REG_ENROLLMENT")
        {
            GridTableView masterTable = item.OwnerTableView;
            if (masterTable != null)
            {
                GridBoundColumn regOwnerTypeIdColumn = (GridBoundColumn)masterTable.GetColumn("ENROLLMENT_IDD");
                if (regOwnerTypeIdColumn != null)
                {
                    regOwnerTypeIdColumn.ReadOnly = true;
                    regOwnerTypeIdColumn.Visible = true;
                }
            }
        }
    }

    private void AddError(string errMsg, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
    }

    protected void DynamicGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "InitInsert")
            {
                GridDataItem item = e.Item as GridDataItem;
                //OHPNM-15112 Making "REG_ENROLLMENT" column as ReadOnly as false during Insert Operation
                if (tableDropdownId.SelectedValue == "REG_ENROLLMENT")
                {
                    GridBoundColumn ENROLLMENT_ID = (GridBoundColumn)DynamicGrid1.MasterTableView.GetColumn("ENROLLMENT_IDD");
                    if (ENROLLMENT_ID != null)
                    {
                        ENROLLMENT_ID.ReadOnly = false;
                        ENROLLMENT_ID.Visible = true;
                    }

                }
            }

            if (e.CommandName == "PerformInsert")
            {
                if (e.Item is GridEditableItem)
                {
                    var editableItem = e.Item as GridEditableItem;
                    Hashtable hashTable = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(hashTable, editableItem);
                    string userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

                    if (tableDropdownId.SelectedItem.Text.ToLower() == "update provider type")
                    {
                        ViewState["EditedItemHashTable"] = hashTable;
                        ViewState["EditedItemNPI"] = string.Empty;
                        ViewState["EditedItemUserId"] = userId;
                        ViewState["InsertAction"] = 1;

                        mpeConfirmActions.Show();
                    }
                    else
                    {
                        DynamicGridInsertData(hashTable, userId);
                    }

                }
            }
            LoadDataForDynamicGrid1();
        }
        catch (Exception ex)
        {
            AddError(ex.Message +" "+ex.StackTrace.ToString(), "DataFix");
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        // Reset dropdowns
        actionDropDown.SelectedIndex = 0;
        tableDropdownId.SelectedIndex = 0;

        // Reset textboxes
        txtRegId.Text = string.Empty;
        txtMedId.Text = string.Empty;
        txtProcessID.Text = string.Empty;
        txtEndDate.Text = string.Empty;
        txtTaskID.Text = string.Empty;

        // Reset checkbox
        chkSendHistory.Checked = false;

        // Reset MultiView to default view
        mltDataFixes.ActiveViewIndex = 0;

        // Disable OK button as initial state
        btnSearch.Enabled = false;

        // Optionally clear or rebind the grid
        DynamicGrid1.DataSource = null;
        DynamicGrid1.Rebind();

        // Clear validation summary if needed
        valSummaryDataFix.Visible = false;
        this.txtProcessID.Visible = false;
        this.txtTaskID.Visible = false;
        this.lblProcessID.Visible = false;
        this.lblTaskID.Visible = false;
        this.txtEndDate.Visible = false;
        this.lblEndDate.Visible = false;

        SetActiveView(actionDropDown.SelectedValue);
    }

    private bool DynamicGridInsertData(Hashtable hashTable, string userId, string recordStatus = "A")
    {
        bool isError = false;
        if (hashTable != null && hashTable.Count > 0)
        {
            if (this.tableDropdownId.SelectedValue.Length > 0)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                PDMSService.PDMSServiceClient _spa = new PDMSService.PDMSServiceClient();
                DataSet dataSet = _spa.GetTableColumnsByTableName(tableDropdownId.SelectedValue, CON.TableOperations.Insert);
                DataTable dtdatafixtable = dataSet.Tables[0];
                string columnName = "";
                string datatype = "";
                string isNotNullable = "";
                string isPopulated = "NO";
                string remainingNull = "";

                foreach (DataRow dftbs in dataSet.Tables[0].Rows)
                {
                    columnName = dftbs["COLUMN_NAME"].ToString();
                    datatype = dftbs["DATA_TYPE"].ToString();
                    isNotNullable = dftbs["IS_NULLABLE"].ToString();
                    isPopulated = "NO";

                    if (datatype.Equals("int") && (columnName.Equals("REG_ID") || columnName.Equals("REGID")))
                    {
                        if (!string.IsNullOrEmpty(txtRegId.Text))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Int32, Convert.ToInt32(txtRegId.Text), true));
                            isPopulated = "YES";
                        }
                    }
                    else if (datatype.Equals("int") && !(columnName.Equals("REG_ID") || columnName.Equals("REGID")))
                    {
                        if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Int32, Convert.ToInt32(hashTable[columnName]), true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("bit"))
                    {
                        if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Boolean, (bool)hashTable[columnName], true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("time"))
                    {
                        if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Time, hashTable[columnName], true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("datetime"))
                    {
                        if (columnName.Equals("Created_On_Date_Time") || columnName.Equals("CREATE_DATE_TIME") || columnName.Equals("CREATED_ON_DATE_TIME") || columnName.Equals("LAST_MODIFIED_DATE_TIME"))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.DateTime, DateTime.Now, true));
                            isPopulated = "YES";
                        }
                        else
                        {
                            if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                            {
                                parameters.Add(SqlParms.CreateParameter(columnName, DbType.DateTime, hashTable[columnName].ToString(), true));
                                isPopulated = "YES";
                            }
                        }
                    }
                    if (datatype.Equals("varchar") || datatype.Equals("nvarchar"))
                    {
                        if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.String, hashTable[columnName].ToString(), true));
                            isPopulated = "YES";
                        }
                    }
                    if (datatype.Equals("uniqueidentifier"))
                    {
                        if (columnName.Equals("Created_By_User") || columnName.Equals("CREATED_BY_USER") || columnName.Equals("CREATED_BY") || columnName.Equals("LAST_MODIFIED_USER"))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Guid, userId, true));
                            isPopulated = "YES";
                        }
                        else
                        {
                            if (hashTable.ContainsKey(columnName) && hashTable[columnName] != null)
                            {
                                parameters.Add(SqlParms.CreateParameter(columnName, DbType.Guid, hashTable[columnName], true));
                                isPopulated = "YES";
                            }
                        }
                    }

                    if (tableDropdownId.SelectedItem.Text.ToLower() == "update provider type" && columnName.ToLower() == "record_status")
                    {
                        if (hashTable.ContainsKey(columnName))
                        {
                            parameters.Add(SqlParms.CreateParameter(columnName, DbType.Guid, recordStatus, true));
                            isPopulated = "YES";
                        }
                    }

                    if (isPopulated.Equals("NO") && isNotNullable.Equals("NO"))
                    {
                        remainingNull = remainingNull + " | " + columnName;
                    }
                    if (tableDropdownId.SelectedValue == "REG_AFFILIATION" && columnName == "MEDICAID_ID"
                        && hashTable.ContainsKey(columnName) && hashTable[columnName] == null)
                    {

                        remainingNull = "Medicaid ID";
                    }                    
                }
                if (string.IsNullOrEmpty(remainingNull))
                {
                    DataAccess.ExecuteStoredProcedure("usp_pnminsert" + this.tableDropdownId.SelectedValue, parameters);

                    string notes = "Insert Data on table - " + this.tableDropdownId.SelectedValue;
                    SaveProviderFeed(notes, userId);

                    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                    svc.SaveLookupDataUpdateTrace(tableDropdownId.SelectedValue, recordStatus, userId);

                }
                else
                {
                    isError = true;
                    AddError("Remaining Fields to be populated : " + remainingNull, "DataFix");
                }
            }
        }

        LoadDataForDynamicGrid1();

        return isError;
    }
}
