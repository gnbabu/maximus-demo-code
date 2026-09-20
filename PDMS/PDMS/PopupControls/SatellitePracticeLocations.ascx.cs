using MAXIMUS.Core.Libraries;
using Models.Data;
using StructureMap.Diagnostics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_SatellitePracticeLocations : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private int _addressTypeId = CON.AddressType.AlternateServiceLocation;
    private int _addressRegId = -1;
    private Address _address = new Address();
    public string SaveButtonClientID
    {
        get;
        set;
    }

    private int _inMaintenance = -1;
    private bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
    }
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }
    #endregion

    private void SetVisibleFields()
    {
        try 
        { 
            ucAddress.ContactVisible = false;
            ucAddress.ZipExtVisible = true;
            ucAddress.IsZipExtRequired = true;
            ucAddress.Email1Visible = false;
            ucAddress.Email2Visible = false;
            ucAddress.Phone1Visible = true;
            ucAddress.Phone2Visible = true;
            ucAddress.PhoneExt1Visible = true;
            ucAddress.PhoneExt2Visible = true;
            ucAddress.VerifyAddress = false;
            ucAddress.EffectiveAndEndDateVisible = true;

            // OHPNM-1917
            if (inMaintenance(this.WorkflowPage.RegistrationId))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                // store off if they are in maintenance, so we can pass to sub-screens to disable them when needed ... not really sure why this screen isn't disabled in maintenance though =/
                _inMaintenance = 1;
            }
            else
            {
                _inMaintenance = 0;
            }

            //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
            if (Session["ViewProviderFile"] != null)
            {
                if (Convert.ToBoolean(Session["ViewProviderFile"]))
                {
                    Helper.SetReadOnly(this, true, "formFieldReadOnly");
                    btnAddSatellitePracticeLocations.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location Page for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    public int? RegAddessId
    {
        get
        {
            return ViewState["RegAddessId"] == null ? 0 : Convert.ToInt32(ViewState["RegAddessId"]);
        }
        set
        {
            ViewState["RegAddessId"] = value;
        }
    }
    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.Other);
            if (Helper.HasRows(ds))
            {
                hidID.Value = ds.Tables[0].Rows[0]["REG_ADDRESS_ID"].ToString();
            }
        }
        else
        {
            hidID.Value = row["REG_ADDRESS_ID"].ToString();
        }
    }

    private bool Set_ID()
    {
        string status = "false";
        DataSet ds = svc.SelectProviderReturnStatus(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            status = ds.Tables[0].Rows[0]["Return_Status"].ToString();
        }
        if (status == "true")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void setVisibilty()
    {
        bool str = Set_ID();
        if (str)
        {
            ucAddress.EffectiveAndEndDateVisible = true;
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 =
            prov_override.Enabled =
            prov_Name.Enabled =
            ucAddress.EnableCounty =
            ucAddress.EnablePhoneNo1 =
            ucAddress.EnablePhoneNo2 =
            ucAddress.EnablePhoneExt1 =
            ucAddress.EnablePhoneExt2 = false;
            
        }
    }

    private void SaveAddressDetails()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(hidID.Value))
            {
                Set_hidID(null);
            }
            _address.CopyPropertiesFrom(ucAddress);
            _address.RegId = this.WorkflowPage.RegistrationId;
            _address.AddressTypeId = _addressTypeId;
            // Had to remove the "String.IsNullOrEmpty(_address.OrgName)" part of this because it would never allow for a 2nd address to be saved with the correct name
            if (!String.IsNullOrEmpty(prov_Name.Text))
            {
                _address.OrgName = prov_Name.Text;
                _address.ContactType = "B";
            }

            var parms = _address.CreateParameterList(_address);
            bool isEdit = !string.IsNullOrWhiteSpace(hdnOtherServiceAddress.Value);

            if (isEdit)
            {
                _addressRegId = (int)RegAddessId;
                parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
                parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
                _address.Update(_address, parms);
            }
            else
            {
                if (WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
                    parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Inserted.ToString());
                else
                    parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.NoChange.ToString());
                parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
                _addressRegId = _address.Insert(_address, parms);
            }
            _address.AddressId = _addressRegId;
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location SaveAddressDetails for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }
    private void LoadAddressDetails(DataRow addressRow)
    {
        bool isEdit = false; 
        try
        {
            isEdit = addressRow != null;
            DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.AlternateServiceLocation);
            bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
            //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
            DataRow drProvider = dsProvider.Tables[0].Rows[0];

            if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
            {
                ucAddress.EnableStreetAddress =
                ucAddress.EnableUnitAddress =
                ucAddress.EnableCity =
                ucAddress.EnableState =
                ucAddress.EnableZip5 =
                ucAddress.EnableZip4 = true;
            }
            else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
            {
                ucAddress.EnableStreetAddress =
                ucAddress.EnableUnitAddress =
                ucAddress.EnableCity =
                ucAddress.EnableState =
                ucAddress.EnableZip5 =
                ucAddress.EnableZip4 = !regIsPending;
            }
            else
            {
                ucAddress.EnableStreetAddress =
                ucAddress.EnableUnitAddress =
                ucAddress.EnableCity =
                ucAddress.EnableState =
                ucAddress.EnableZip5 =
                ucAddress.EnableZip4 = false;
            }

            // hdnRegWorkHistoryId.Value = isEdit.ToString();
            ucAddress.LoadState();
            if (addressRow != null)
            {
                _address.Load(addressRow, drProvider);
                ucAddress.CopyPropertiesFrom(_address);
                if (Methods.Exists(addressRow, "ADDRESS_VALIDATION_OVERRIDE"))
                {
                    var avo = addressRow["ADDRESS_VALIDATION_OVERRIDE"].ToString();
                    if (avo == "1")
                        prov_override.Checked = true;
                    else
                        prov_override.Checked = false;
                }
            }
            else
            {
                _address.Load(drProvider);
            }

            ucAddress.NameSectionVisible = ucAddress.IsIndividual;
            ucAddress.OrgNameVisible = !ucAddress.IsIndividual;

            _address.CanText1 = ucAddress.CanText1;
            _address.CanText2 = ucAddress.CanText2;


            if (isEdit)
            {

                hidID.Value = Helper.GetInt("REG_ADDRESS_ID", addressRow).ToString();
                _addressRegId = Helper.GetInt("REG_ADDRESS_ID", addressRow);
            }
            else
            {
                hidID.Value = string.Empty;
                _addressRegId = -1;
            }

            ucAddress.RegId = this.WorkflowPage.RegistrationId;
            ucAddress.PageTitle = CON.AddressPage.Other;

            ucAddress.AddressTypeId = _addressTypeId;
            ucAddress.SaveButtonClientID = SaveButtonClientID;
            SetVisibleFields();
            // setVisibilty();   //SAM538/OHPNM-19400 - Allow to add/update primary or other service locations when RTP from Site Visit
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location LoadAddressDetails for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    protected void RadGrid1_GridSortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        try
        {
            string cmdArg = e.CommandArgument.ToString();
            string cmdSort = e.NewSortOrder.ToString();
            int pageID = rgSatellitePracticeLocations.CurrentPageIndex + 1;
            int RowsPerPage = rgSatellitePracticeLocations.PageSize;

            DataTable dt = PopulateGridSorted(pageID, RowsPerPage, cmdSort, cmdArg);

            rgSatellitePracticeLocations.DataSource = null;
            rgSatellitePracticeLocations.DataSource = this.DataList = dt;
            rgSatellitePracticeLocations.DataBind();
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location RadGrid1_GridSortCommand for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    public override bool SaveData()
    {
        try
        {
            ucAddress.VerifyAddress = true;
            ucAddress.OverrideAddressValidation = prov_override.Checked;
            Page.Validate("SatellitePracticeLocations");

            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("SatellitePracticeLocations") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }
            if (satelliteLocDetail.Visible)
            {
                if (ValidateData())
                {
                    SaveAddressDetails();
                    if (_address.AddressId > 0)
                    {
                        OfficeHours.SaveData(_address.AddressId);

                        ucAddress.Confirmed = true;
                        SetVisibleFields();

                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location SaveData for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
        return false;
    }

    protected string FormatPhone(object phn)
    {
        string phone = phn is string ? phn.ToString() : string.Empty;
        return Helper.FormatPhone(phone);
    }
    protected string FormatAddress(object add1, object add2, object city, object state, object zip, object ext_zip)
    {

        return Helper.GetFormattedAddress(Helper.ConvertNull(add1).ToString(), Helper.ConvertNull(add2).ToString(), Helper.ConvertNull(city).ToString(), Helper.ConvertNull(state).ToString(), Helper.ConvertNull(zip).ToString(), Helper.ConvertNull(ext_zip).ToString(), string.Empty);
    }


    protected void grd_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void grd_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("Page") || e.CommandName.Equals("Filter") || e.CommandName.Equals("ChangePageSize") || e.CommandName.Equals("Sort"))
            {
                return;
            }
            GridDataItem item = (GridDataItem)e.Item;

            HiddenField hdnAddressId = item.FindControl("hdnRegAddressId") as HiddenField;

            int index = Convert.ToInt32(e.CommandArgument);
            DataTable dt = new DataTable();
            dt = dtSatellitePracticeLocationsnewbyID(index);
            satelliteLocDetail.Visible = true;
            if (Helper.HasRows(dt))
                this.LoadData(dt.Rows[0]);
            else
                this.LoadData(null);

            RegAddessId = !string.IsNullOrWhiteSpace(hdnAddressId.Value) ? int.Parse(hdnAddressId.Value) : (int?)null;
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location RadGrid1_ItemCommand for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        try
        {
            int totalResultCount = 0;
            DataTable dataTable = new DataTable();
            dataTable = this.dtSatellitePracticeLocationsnew;
            GridView GridView1 = new GridView();
            GridView1.DataSource = dtSatellitePracticeLocationsnew;
            GridView1.DataBind();
            ExportX(GridView1);
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location lnkExcel_Click for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    private static void PrepareControlForExport(Control control)
    {
        try
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location PrepareControlForExport for Error : {0} {1}", ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }
    private void ExportX(GridView GridView1)
    {
        try
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; Filename = ExcelReport.xls"));
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWrite);
            Table table = new Table();

            //  include the gridline settings
            table.GridLines = GridView1.GridLines;
            if (GridView1.HeaderRow != null)
            {
                PrepareControlForExport(GridView1.HeaderRow);
                table.Rows.Add(GridView1.HeaderRow);
            }
            foreach (GridViewRow row in GridView1.Rows)
            {
                PrepareControlForExport(row);
                table.Rows.Add(row);
            }
            if (GridView1.FooterRow != null)
            {
                PrepareControlForExport(GridView1.HeaderRow);
                table.Rows.Add(GridView1.FooterRow);
            }

            table.RenderControl(htmlWriter);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location ExportX for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }
    

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        try
        {   
            satelliteLocDetail.Visible = true;
            this.LoadData(null);
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location lbtnAdd_Click for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    public override void LoadData(DataRow row)
    {
        try
        {
            LoadAddressDetails(null);
            bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
            if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
            {
                prov_Name.Enabled =
               ucAddress.Phone1Visible = ucAddress.PhoneExt1Visible = true;
            }
            else  if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator))
            {
                prov_Name.Enabled =
                ucAddress.Phone1Visible = ucAddress.PhoneExt1Visible = !regIsPending;
            }
            else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            {
                prov_Name.Enabled = ucAddress.Phone1Visible = ucAddress.PhoneExt1Visible = true;
            }
            // OHPNM - 2540
            else if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
            {
                prov_Name.Enabled = ucAddress.Phone1Visible = ucAddress.PhoneExt1Visible = true;
            }
		    //OHPNM-3200
            else
            {
                prov_Name.Enabled = ucAddress.Phone1Visible = ucAddress.PhoneExt1Visible = true;
            }

            hidIsEdit.Text = (row != null).ToString();

            if (row != null)
            {
                LoadAddressDetails(row);
                hidID.Value = row["REG_ADDRESS_ID"].ToString();
                hdnOtherServiceAddress.Value = Helper.GetData("REG_ADDRESS_ID", row);
                prov_Name.Text = row["PRACTICE_NAME"].ToString();
			    OfficeHours.LoadOfficeInformation(this.WorkflowPage.RegistrationId,Helper.GetInt("REG_ADDRESS_ID", row), _inMaintenance);
                if (Methods.Exists(row, "ADDRESS_VALIDATION_OVERRIDE"))
                {
                    var avo = row["ADDRESS_VALIDATION_OVERRIDE"].ToString();
                    if (avo == "1")
                        prov_override.Checked = true;
                    else
                        prov_override.Checked = false;
                }
            }
            else
            {
                _address.AddressId = _addressRegId = -1;
                RegAddessId = null;
                hdnOtherServiceAddress.Value = null;
                hidID.Value = string.Empty;
                prov_Name.Text = string.Empty;
                ucAddress.StreetAddress = string.Empty;
                ucAddress.UnitAddress = string.Empty;
                ucAddress.City = string.Empty;
                ucAddress.State = string.Empty;
                ucAddress.Zip5 = string.Empty;
                ucAddress.Zip4 = string.Empty;
                ucAddress.PhoneNumber1 = string.Empty;
                ucAddress.PhoneNumber2 = string.Empty;
                ucAddress.PhoneExt1 = string.Empty;
                ucAddress.PhoneExt2 = string.Empty;
                ucAddress.LoadState();
			    OfficeHours.LoadOfficeInformation(this.WorkflowPage.RegistrationId ,-1, _inMaintenance);

            }
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location LoadData for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    public override bool ValidateData()
    {
        bool isValid = false; 
        try
        {
            if (OfficeHours.Visible)
            {
                isValid = true;

                // validate office hours data if applicable
                if (OfficeHours.HasInputValue() && OfficeHours.ValidateData() == false)
                {
                    isValid = false;
                    return isValid;
                }
            }
            else if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
                isValid = true;
            // Duplicate address check
            if (Helper.HasRows(this.DataList))
            {
                var duplicate = from d in this.DataList.AsEnumerable()
                                where (d.Field<string>("ADDRESS1") ?? "").Trim().ToUpper() + " " + (d.Field<string>("ADDRESS2") ?? "").Trim().ToUpper() == (ucAddress.StreetAddress ?? "").Trim().ToUpper() + " " + (ucAddress.UnitAddress ?? "").Trim().ToUpper()
                                && (d.Field<string>("CITY") ?? "").Trim().ToUpper() == (ucAddress.City ?? "").Trim().ToUpper()
                                && (d.Field<string>("STATE") ?? "").Trim().ToUpper() == (ucAddress.State ?? "").Trim().ToUpper()
                                && (d.Field<string>("ZIP") ?? "").Trim() == (ucAddress.Zip5 ?? "").Trim()
                                && (d.Field<string>("EXT_ZIP") ?? "").Trim() == (ucAddress.Zip4 ?? "").Trim()
                                && Methods.FormatPhone((d.Field<string>("PHONE1") ?? "").Trim()) == (ucAddress.PhoneNumber1 ?? "").Trim()
                                && (d.Field<DateTime?>("ADR_END_DATE").ToString() ?? "12/31/2299").Trim() == (ucAddress.EndDate ?? "").Trim()                            
                                && (d.Field<int>("REG_ADDRESS_ID")) != RegAddessId
                                select d;
                if (duplicate.Count() > 0)
                {
                    isValid = false;
                    ucAddress.VisibleDivAddressDupllicate = true;
                }
                else
                {
                    isValid = true;
                    ucAddress.VisibleDivAddressDupllicate = false;
                }
            }
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location ValidateData for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
        return isValid;
    }
    protected override void OnLoad(EventArgs e)
    {
        try
        {
            LoadControlData();
            base.OnLoad(e);
            SetVisibleFields();
            Page.Title = "Other Service Locations";
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location OnLoad for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }
    public override void LoadControlData()
    {
        LoadSatelliteLocations();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.AlternateServiceLocation.ToString());
            DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grd.DataSource = ds.Tables[0];
                grd.DataBind();
                grd.MasterTableView.ExportToExcel();
            }
        }
    }

    public DataTable dtSatellitePracticeLocations
    {
        get
        {
            try
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectAddressCustomData(this.WorkflowPage.RegistrationId, CON.AddressType.AlternateServiceLocation, "ADDRESSCustom");
                DataTable _dtSatellitePracticeLocations = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
                return _dtSatellitePracticeLocations;
            }
            catch (Exception ex)
            {
                Logging log = new Logging();
                log.CreateLogEntry(string.Format("Other Service Location dtSatellitePracticeLocations for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                return null;
            }
        }
    }
    public DataTable dtSatellitePracticeLocationsnew
    {
        get 
        { 
            try
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectAddressCustomData(this.WorkflowPage.RegistrationId, CON.AddressType.AlternateServiceLocation, "ADDRESSCustomNew");
                DataTable _dtSatellitePracticeLocations = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
                return _dtSatellitePracticeLocations;
            }
            catch (Exception ex)
            {
                Logging log = new Logging();
                log.CreateLogEntry(string.Format("Other Service Location dtSatellitePracticeLocationsnew for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                return null;
            }
        }
    }

    public DataTable dtSatellitePracticeLocationsnewbyID(int regAddrID)
    {
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectAddressADDRESSCustomByID(this.WorkflowPage.RegistrationId, CON.AddressType.AlternateServiceLocation, regAddrID);
            DataTable _dtSatellitePracticeLocations = Helper.HasRows(ds) ? ds.Tables["RegistrationAddressDataByID"] : null;
            return _dtSatellitePracticeLocations;
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location dtSatellitePracticeLocationsnewbyID for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            return null;
        }
    }

    private DataTable PopulateGridSorted(int pageID = 1, int RowsPerPage = 50, string sortOrder = "Ascending", string sortColumn = "SortPracticeName")
    {
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectAddressCustomDataSortedNew(this.WorkflowPage.RegistrationId, CON.AddressType.AlternateServiceLocation, pageID, RowsPerPage, sortOrder, sortColumn);
            DataTable _dtSatellitePracticeLocations = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            return _dtSatellitePracticeLocations;
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location PopulateGridSorted for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            return null;
        }
    }

    private DataTable PopulateGrid(int pageID = 1, int RowsPerPage = 50)
    {
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectAddressCustomDataPagingNew(this.WorkflowPage.RegistrationId, CON.AddressType.AlternateServiceLocation, pageID, RowsPerPage);
            DataTable _dtSatellitePracticeLocations = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            return _dtSatellitePracticeLocations;
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location PopulateGrid for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            return null;
        }
    }

    private void LoadSatelliteLocations(int pageID = 1, int RowsPerPage = 10)
    {
        try
        {
            int pageIDn = rgSatellitePracticeLocations.CurrentPageIndex + 1;
            int RowsPerPagen = rgSatellitePracticeLocations.PageSize;
            DataTable dt = PopulateGrid(pageIDn, RowsPerPagen);
            DataView dv;
            int totalRows = 0;
            if (Methods.HasRows(dt))
            {
                dv = dt.DefaultView;
                totalRows = Convert.ToInt32(dv[0]["TotalRows"]);
            }

            rgSatellitePracticeLocations.DataSource = null;
            rgSatellitePracticeLocations.VirtualItemCount = totalRows;
            rgSatellitePracticeLocations.DataSource = this.DataList = dt;
            rgSatellitePracticeLocations.DataBind();
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location LoadSatelliteLocations for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    protected void RadGrid1_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        try
        {
            LoadSatelliteLocations(1, e.NewPageSize);
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location RadGrid1_PageSizeChanged for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    public SortDirection dirSatellitePracticeLocations
    {
        get
        {
            if (ViewState["dirStateSatellitePracticeLocations"] == null)
            {
                ViewState["dirStateSatellitePracticeLocations"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirStateSatellitePracticeLocations"];
        }
        set
        {
            ViewState["dirStateSatellitePracticeLocations"] = value;
        }
    }

    protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            rgSatellitePracticeLocations.CurrentPageIndex = e.NewPageIndex;
            LoadSatelliteLocations(e.NewPageIndex + 1, 10);
        }
        catch (Exception ex)
        {
            Logging log = new Logging();
            log.CreateLogEntry(string.Format("Other Service Location RadGrid1_PageIndexChanged for REG_ID : {0} Error {1} {2}", this.WorkflowPage.RegistrationId, ex.Message, ex.StackTrace), Logging.LogPriority.Error);
        }
    }

    public override bool HasInputValue()
    {
        bool isRequired = false;

        if (satelliteLocDetail.Visible)
        {
            // there are fields on the screen, check to see if they are filled out
            isRequired = !string.IsNullOrWhiteSpace(this.prov_Name.Text) || !string.IsNullOrWhiteSpace(this.ucAddress.StreetAddress)
                                                                     || !string.IsNullOrWhiteSpace(this.ucAddress.City) ||
                                                                     !string.IsNullOrWhiteSpace(Helper.StripNonNumerics(ucAddress.PhoneNumber1));
        }
        else if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
            isRequired = true;

        // they haven't clicked "Add New", so there's no fields on the screen to check if they are filled out
        //isRequired = false;

        return isRequired;
    }

    public override string ValidationGroup
    {
        get { return "SatellitePracticeLocations"; }
    }

    public override string Title
    {
        get { return "Satellite Location"; }
    }

    public override string IdText
    {
        get { return "ucSatellitePracticeLocations_" + this.WorkflowPage.RegistrationId; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        hdnRegId.Value = this.WorkflowPage.RegistrationId.ToString();
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "otherServiceLocations";
        }
        else
        {
            if (Session["Override_prop"] != null)
            {
                if (Session["Override_prop"].ToString() == "primaryContactAddress" ||
                    Session["Override_prop"].ToString() == "primaryServiceAddress" || 
                    Session["Override_prop"].ToString() == "CorrespondenceAddress" ||
                    Session["Override_prop"].ToString() == "billingPaymentAddress" ||
                    Session["Override_prop"].ToString() == "homeOfficeAddress" ||
                    Session["Override_prop"].ToString() == "NursingFacilityAddress")
                {
                    Session["Override_prop"] = "False";
                }
            }
            else { Session["Override_prop"] = "False"; }
        }
        prov_override.InputAttributes.Add("aria-label", "Override Address Validation");
		ucAddress.AddressTypeId = _addressTypeId;

        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview && Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
        {
            if (rgSatellitePracticeLocations.Items.Count > 0)
            {
                btnBulkApproveMain.Visible = true;
            }
        }
    }

    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "otherServiceLocations";
        }
        else
        {
            Session["Override_prop"] = "False";
        }

    }

    protected void lnkExcelHistory_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }


    protected void btnBulkApproveMain_Click(object sender, EventArgs e)
    {
        rgSatellitePracticeLocations.Columns[0].Visible = true;
        btnBulkApprove.Visible = true;
        btnBulkDeny.Visible = true;
        btnBulkApproveMain.Visible = false;
    }

    protected void btnBulkApprove_Click(object sender, EventArgs e)
    {
        var providerRegIDs = new List<int>();
        foreach (GridDataItem item in rgSatellitePracticeLocations.MasterTableView.Items)
        {
            CheckBox chkItem = (CheckBox)item.FindControl("chkAssign");
            HiddenField hdnRegAddressId = (HiddenField)item.FindControl("hdnRegAddressId");

            if (chkItem != null && chkItem.Checked && hdnRegAddressId != null)
            {
                var regIdObj = hdnRegAddressId.Value;
                int regId = regIdObj != null ? Convert.ToInt32(regIdObj) : 0;
                providerRegIDs.Add(regId);
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mainDB"].ConnectionString))
        using (SqlCommand cmd = new SqlCommand("usp_UpdateOtherServiceLocStatusBulk", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            // Create table-valued parameter
            DataTable tvp = new DataTable();
            tvp.Columns.Add("Value", typeof(int));
            foreach (int id in providerRegIDs)
            {
                tvp.Rows.Add(id);
            }

            cmd.Parameters.AddWithValue("@RegIDs", tvp);
            cmd.Parameters.AddWithValue("@NewStatus", "1"); // 1 approve

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        LoadSatelliteLocations();
        rgSatellitePracticeLocations.Rebind();
        UpdatePanelSatellitePractice.Update();
    }

    protected void btnBulkDeny_Click(object sender, EventArgs e)
    {
        var providerRegIDs = new List<int>();
        foreach (GridDataItem item in rgSatellitePracticeLocations.MasterTableView.Items)
        {
            CheckBox chkItem = (CheckBox)item.FindControl("chkAssign");
            HiddenField hdnRegAddressId = (HiddenField)item.FindControl("hdnRegAddressId");

            if (chkItem != null && chkItem.Checked && hdnRegAddressId != null)
            {
                var regIdObj = hdnRegAddressId.Value;
                int regId = regIdObj != null ? Convert.ToInt32(regIdObj) : 0;
                providerRegIDs.Add(regId);
            }
        }

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mainDB"].ConnectionString))
        using (SqlCommand cmd = new SqlCommand("usp_UpdateOtherServiceLocStatusBulk", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            // Create table-valued parameter
            DataTable tvp = new DataTable();
            tvp.Columns.Add("Value", typeof(int));
            foreach (int id in providerRegIDs)
            {
                tvp.Rows.Add(id);
            }

            cmd.Parameters.AddWithValue("@RegIDs", tvp);
            cmd.Parameters.AddWithValue("@NewStatus", "2"); // 2 deny

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        LoadSatelliteLocations();
        rgSatellitePracticeLocations.Rebind();
        UpdatePanelSatellitePractice.Update();
    }
}