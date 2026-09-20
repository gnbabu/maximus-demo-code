using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_DiagnosisCodePanel : System.Web.UI.UserControl
{
    public delegate void EventHandler();
    public event EventHandler RefreshChildDropDown;
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


    #region 'properties'
    private ClaimsServiceAgent ClaimService = null;
    private DataSet dsDiagnosisCodes = new DataSet();
    public string ICNNumber = string.Empty;
    private bool _displayReadOnly;
    DataSet dsDiagCode = new DataSet();
    private bool fromInquirySvc = false;

    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ICNNumber))
                return ICNNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ICNNumber = value.Trim();
        }
    }

    public bool FromInquirySvc
    {
        get { 
            return fromInquirySvc;
        }
        set
        {
            fromInquirySvc = value;
        }
    }

    public string ClaimID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimIdDiagnosis.Value))
                return hdnClaimIdDiagnosis.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimIdDiagnosis.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimType_Diagnosis.Value))
                return hdnClaimType_Diagnosis.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                hdnClaimType_Diagnosis.Value = value.Trim();
                SetFieldsVisibility(value);
            }
        }
    }

    public DataSet dataSetDiagnosisCode
    {
        get
        {
            if (!Helper.HasRows(dsDiagnosisCodes))
            {
                if (Helper.HasRows(LoadDiagnosisData()))
                {
                    return dsDiagnosisCodes;
                }
            }
            return dsDiagnosisCodes;
        }

        set
        {
            if (value != null)
            {
                GetDiagCode(value);
            }
        }
    }

    public bool DisplayReadOnly
    {
        get
        {
            return _displayReadOnly;
        }
        set
        {
            _displayReadOnly = value;
            // BindGrid();
        }
    }


    public string DdlICDVersion
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlICDVer.SelectedValue))
                return ddlICDVer.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && ddlICDVer.Items.FindByValue(value) != null)
            {
                ddlICDVer.ClearSelection();
                ddlICDVer.SelectedValue = value.Trim();
            }
        }
    }

    public string SequenceDesc
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ddlDiagnosisSequenceDescription.SelectedValue))
                return ddlDiagnosisSequenceDescription.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && ddlDiagnosisSequenceDescription.Items.FindByValue(value) != null)
                ddlDiagnosisSequenceDescription.ClearSelection();
            ddlDiagnosisSequenceDescription.SelectedValue = value.Trim();
        }
    }

    public string DiagnosisCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtDiagnosisCode.Text))
                return txtDiagnosisCode.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtDiagnosisCode.Text = value.Trim();
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        lblDeleteError.Text = "";

        if (!IsPostBack)
        {
            try
            {
                DropdownBind_ICDVersions();
                LoadSeqenceDescription();
                LoadPresentOnAdmission();
                // GetDiagnosisData();
            }

            catch (Exception ex)
            {

            }
        }
        else if (!string.IsNullOrEmpty(hdnClaimType_Diagnosis.Value))
        {
            SetFieldsVisibilityClaimtype(hdnClaimType_Diagnosis.Value);
        }

        btnDiagnosis.Attributes.Add("onclick", "DiaDisableEnableConditionAddButton();");

        // DiagnosisGridviewCountValidation();
        SetButtonVisibility();
        if (!string.IsNullOrEmpty(hdnClaimIdDiagnosis.Value) && !FromInquirySvc)
        {
            GetDiagCode(dsDiagnosisCodes);
        }
        if (!(lblDiagDesc.Value == null && lblDiagDesc.Value == ""))
        { lblDiagnosisDescription.Text = lblDiagDesc.Value; }
        if (pnlDiagnosis.Visible && !FromInquirySvc)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displaytable();</script>", false);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>DiagClearFields();</script>", false);
        }

    }

    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            btnDiagnosis.Visible = false;
            btnDiagnosis.Enabled = false;
            divInsert.Visible = false;
        }
        else
        {
            btnDiagnosis.Visible = true;
            btnDiagnosis.Enabled = true;
            divInsert.Visible = true;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(txtDiagnosisCode.Text.ToString()) && ddlICDVer.SelectedIndex == 0 && ddlDiagnosisSequenceDescription.SelectedIndex == 0)
        {
            DropdownBind_ICDVersions();
            LoadSeqenceDescription();
            GetDiagCode(null);
            //  GetDiagnosisData();
        }
    }

    #region 'Panel Save'
    protected void btnDiagnosisAdd_Click(object sender, EventArgs e)
    {
        lblDeleteError.Text = "";
        if (Page.IsValid)
        {
            lblIcdVersionError.Text = "";
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimIdDiagnosis.Value);
            parms.Add("Claim_Type", hdnClaimType_Diagnosis.Value);
            DataSet dsDiagnosis = svc.SelectPanelsData("Claims_Diagnosis_Information", parms);
            parms.Clear();

            if (ClaimType == CON.ClaimsType.Institutional &&
                (ddlDiagnosisSequenceDescription.SelectedItem.Text == CON.Sequences.Principal || ddlDiagnosisSequenceDescription.SelectedItem.Text == "Other" || ddlDiagnosisSequenceDescription.SelectedItem.Text == "External Cause of Injury")
                && String.IsNullOrEmpty(ddlPresentAdmission.SelectedItem.Text.ToString()))
            {
                lblPresentOnAdmissionEror.Text = "Present on Admission is required";
                return;
            }
            else
            {
                lblPresentOnAdmissionEror.Text = string.Empty;
            }

            if (string.IsNullOrEmpty(txtDiagnosisCode.Text))
            {
                lblIcdVersionError.Text = "*Diagnosis Code is required";
                return;
            }


            //Inserting on DB table
            if (!string.IsNullOrEmpty(txtDiagnosisCode.Text) && !string.IsNullOrEmpty(hdnClaimIdDiagnosis.Value))
            {
                int rows = 0;
                string line = "";
                if (Helper.HasRows(dsDiagnosis))
                {
                    rows = dsDiagnosis.Tables[0].Rows.Count;
                }

                line = (rows + 1).ToString();
                parms.Add("Sequence", line);
                parms.Add("Diagnosis_Code", txtDiagnosisCode.Text);
                parms.Add("ICD_Version", ddlICDVer.SelectedItem.Text);
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Claim_ID", hdnClaimIdDiagnosis.Value);


                if (hdnClaimType_Diagnosis.Value == CON.ClaimsType.Institutional)
                {
                    parms.Add("Sequence_Description_ID", ddlDiagnosisSequenceDescription.SelectedValue.ToString());
                    if (!String.IsNullOrEmpty(ddlPresentAdmission.SelectedItem.Text))
                    {
                        parms.Add("Present_On_Admission", ddlPresentAdmission.SelectedItem.Value.Trim());
                    }
                    else
                        parms.Add("Present_On_Admission", string.Empty);
                }
                else
                {
                    parms.Add("Sequence_Description_ID", null);
                    parms.Add("Present_On_Admission", null);
                }

                try
                {
                    svc.InsertPanelsData("Claims_Diagnosis_Information", parms);
                }
                catch (Exception ex) { }
            }

            //displaying binded data from db 
           // GetDiagnosisData();
            LoadSeqenceDescription();
            RefreshChildDropDown();
            ClearDiagnosisCodeFields();
        }
    }
    #endregion

    #region 'GridView Events'
    //protected void gvDiagnosis_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ClaimType == CON.ClaimsType.Dental || ClaimType == CON.ClaimsType.Professional)
    //        {
    //            if (DisplayReadOnly)
    //            {
    //               // gvDiagnosis.Columns[7].Visible = false;
    //                //gvDiagnosis.Columns[5].Visible = false;
    //              //  gvDiagnosis.Columns[6].Visible = false;


    //                divInsert.Visible = false;
    //            }
    //            else
    //            {
    //                //gvDiagnosis.Columns[5].Visible = true;
    //              //  gvDiagnosis.Columns[6].Visible = true;
    //              //  gvDiagnosis.Columns[7].Visible = true;
    //                //divInsert.Visible = true;
    //            }


    //        }
    //        else if (ClaimType == CON.ClaimsType.Institutional)
    //        {
    //            if (DisplayReadOnly)
    //            {
    //               // gvDiagnosis.Columns[7].Visible = false;
    //              //  gvDiagnosis.Columns[6].Visible = false;
    //                //divInsert.Visible = f;
    //            }
    //            else
    //            {
    //              //  gvDiagnosis.Columns[7].Visible = true;
    //              //  gvDiagnosis.Columns[6].Visible = true;
    //                //divInsert.Visible = true;
    //            }

    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //    (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
    //    {
    //        int id = Convert.ToInt32(gvDiagnosis.DataKeys[e.Row.RowIndex].Values[0]);
    //        //Find the DropDownList in the Row
    //        DropDownList drpdownIcdVersion = (e.Row.FindControl("drpdownIcdVersion") as DropDownList);
    //        DataSet dsICD = LookupTableController.GetICDVersions();
    //        drpdownIcdVersion.DataSource = dsICD;
    //        drpdownIcdVersion.DataTextField = "ICD_Versions";
    //        drpdownIcdVersion.DataValueField = "ICD_Versions";
    //        drpdownIcdVersion.DataBind();

    //        DataSet dsDiagnosis = LoadDiagnosisData();
    //        if (Helper.HasRows(dsDiagnosis))
    //        {
    //            var selectedrow = from myrow in dsDiagnosis.Tables[0].AsEnumerable()
    //                     where myrow.Field<Int32>("Claims_Diagnosis_Information_ID") == Convert.ToInt32(id)
    //                     select myrow;
    //            if (selectedrow.Any())
    //            {
    //                DataTable dtdiagcode = selectedrow.CopyToDataTable();
    //                TextBox txtGridDiagnosisCode = e.Row.FindControl("txtGridDiagnosisCode") as TextBox;
    //                Label lblEditDiagnosisDescription = e.Row.FindControl("lblEditDiagnosisDescription") as Label;
    //                if (txtGridDiagnosisCode != null && lblEditDiagnosisDescription != null)
    //                {
    //                    txtGridDiagnosisCode.Text = dtdiagcode.Rows[0]["diag_code"].ToString();
    //                    lblEditDiagnosisDescription.Text = dtdiagcode.Rows[0]["diagnosisDescription"].ToString();
    //                }
    //            }
    //        }
    //        if (ClaimType == CON.ClaimsType.Institutional)
    //        {
    //            DropDownList dropDownSeqDesc = (e.Row.FindControl("dropDownSeqDesc") as DropDownList);
    //            //dropDownSeqDesc.SelectedIndexChanged += new EventHandler(dropDownSeqDesc_SelectedIndexChanged);
    //            Dictionary<string, string> parms = new Dictionary<string, string>();
    //            parms.Add("Claim_Id", hdnClaimIdDiagnosis.Value);
    //            DataSet dsSequenceDesc = svc.SelectPanelsData("claims_sequence_description", parms);
    //            if (dropDownSeqDesc != null)
    //            {
    //                Helper.LoadList(dropDownSeqDesc, dsSequenceDesc, "Sequence_Description", "Claims_Sequence_Description_ID", true);
    //            }
    //            parms.Clear();
    //            String selectedSequenceDesc = String.Empty;
    //            if (ViewState["SelectedSequenceDesc"] != null)
    //                selectedSequenceDesc = ViewState["SelectedSequenceDesc"].ToString();
    //            if (ClaimType == CON.ClaimsType.Institutional)
    //            {
    //                DataSet ds = LoadDiagnosisData();
    //                int PrincipalCount = 0;
    //                int AdmittingCount = 0;
    //                int OtherCount = 0;
    //                int PatientReasonForVisitCount = 0;
    //                int ExternalCauseOfInjuryCount = 0;
    //                if (Helper.HasRows(ds))
    //                {
    //                    foreach (DataRow dr in ds.Tables[0].Rows)
    //                    {
    //                        try
    //                        {
    //                            if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(1))
    //                            {
    //                                PrincipalCount += 1;
    //                            }
    //                            else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(2))
    //                            {
    //                                AdmittingCount += 1;
    //                            }
    //                            else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(3))
    //                            {
    //                                OtherCount += 1;
    //                            }
    //                            else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(4))
    //                            {
    //                                PatientReasonForVisitCount += 1;
    //                            }
    //                            else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(5))
    //                            {
    //                                ExternalCauseOfInjuryCount += 1;
    //                            }
    //                        }
    //                        catch (Exception ex)
    //                        {

    //                        }
    //                    }
    //                }
    //                foreach (ListItem item in dropDownSeqDesc.Items)
    //                {
    //                    if (item.Value == "1" && PrincipalCount >= 1 && selectedSequenceDesc != "1")
    //                    {
    //                        item.Enabled = false;
    //                    }
    //                    else if (item.Value == "2" && AdmittingCount >= 1 && selectedSequenceDesc != "2")
    //                    {
    //                        item.Enabled = false;
    //                    }
    //                    else if (item.Value == "3" && OtherCount >= 24 && selectedSequenceDesc != "3")
    //                    {
    //                        item.Enabled = false;
    //                    }
    //                    else if (item.Value == "4" && PatientReasonForVisitCount >= 3 && selectedSequenceDesc != "4")
    //                    {
    //                        item.Enabled = false;
    //                    }
    //                    else if (item.Value == "5" && ExternalCauseOfInjuryCount >= 12 && selectedSequenceDesc != "5")
    //                    {
    //                        item.Enabled = false;
    //                    }
    //                }
    //            }


    //            DropDownList drpdownPresentOnAdmission = (e.Row.FindControl("drpdownPresentOnAdmission") as DropDownList);
    //            if (drpdownPresentOnAdmission != null)
    //            {
    //                drpdownPresentOnAdmission.Items.Clear();
    //                drpdownPresentOnAdmission.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    //                drpdownPresentOnAdmission.Items.Insert(1, new ListItem("Yes", "Y"));
    //                drpdownPresentOnAdmission.Items.Insert(2, new ListItem("No", "N"));
    //                drpdownPresentOnAdmission.Items.Insert(3, new ListItem("Unknown", "U"));
    //                drpdownPresentOnAdmission.Items.Insert(4, new ListItem("Not Applicable", "W"));
    //            }

    //            if (!String.IsNullOrEmpty(selectedSequenceDesc) && (selectedSequenceDesc.Equals("2") || selectedSequenceDesc.Equals("4")))
    //            {
    //                drpdownPresentOnAdmission.SelectedIndex = 0;
    //                drpdownPresentOnAdmission.Enabled = false;
    //            }
    //        }
    //    }
    //    if (DisplayReadOnly == true)
    //    {
    //        divInsert.Visible = false;
    //    }
    //    else
    //    {
    //        divInsert.Visible = true;
    //    }
        
    //}

    //protected void gvDiagnosis_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    lblIcdVersionError.Text = "";
    //    string keyVal = gvDiagnosis.DataKeys[e.RowIndex].Value.ToString();
    //    TextBox diagCode = (TextBox)gvDiagnosis.Rows[e.RowIndex].FindControl("txtGridDiagnosisCode");
    //    DropDownList icdVersion = (DropDownList)gvDiagnosis.Rows[e.RowIndex].FindControl("drpdownIcdVersion");
    //    DropDownList dropDownSeqDesc = null;
    //    DropDownList drpdownPresentOnAdmission = null;
      //  if (ClaimType == CON.ClaimsType.Institutional)
    //    {
    //        dropDownSeqDesc = (DropDownList)gvDiagnosis.Rows[e.RowIndex].FindControl("dropDownSeqDesc");
    //        drpdownPresentOnAdmission = (DropDownList)gvDiagnosis.Rows[e.RowIndex].FindControl("drpdownPresentOnAdmission");
    //    }
    //    if (ClaimType == CON.ClaimsType.Institutional &&
    //            (dropDownSeqDesc.SelectedItem.Text == CON.Sequences.Principal || dropDownSeqDesc.SelectedItem.Text == "Other" || dropDownSeqDesc.SelectedItem.Text == "External Cause of Injury")
    //            && String.IsNullOrEmpty(drpdownPresentOnAdmission.SelectedItem.Text.ToString()))
    //    {
    //        lblPresentOnAdmissionEror.Text = "Present on Admission is required";
    //        return;
    //    }
    //    else
    //    {
    //        lblPresentOnAdmissionEror.Text = string.Empty;
    //    }
    //    //Updating db
    //    Dictionary<string, string> parms = new Dictionary<string, string>();
    //    parms.Add("Claims_Diagnosis_Information_ID", (keyVal).ToString());
    //    parms.Add("Diagnosis_Code", diagCode.Text);
    //    parms.Add("ICD_Version", icdVersion.SelectedValue.ToString());
    //    parms.Add("Claim_ID", hdnClaimIdDiagnosis.Value);
    //    parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //    if (ClaimType == CON.ClaimsType.Institutional)
    //    {
    //        parms.Add("Sequence_Description_ID", dropDownSeqDesc.SelectedValue.ToString());
    //        parms.Add("Present_On_admission", drpdownPresentOnAdmission.SelectedValue.ToString());
    //    }
    //    else
    //    {
    //        parms.Add("Sequence_Description_ID", null);
    //        parms.Add("Present_On_admission", null);
    //    }
    //    try
    //    {
    //     svc.UpdatePanelsData("Claims_Diagnosis_Information", parms);
    //    }
    //    catch (Exception ex) { }
    //    parms.Clear();

    //    //Displaying binded data from db 
    //    gvDiagnosis.EditIndex = -1;
    //    GetDiagnosisData();
    //    LoadSeqenceDescription();
    //}

    //protected void gvDiagnosis_RowCancelingEdit(object sender, EventArgs e)
    //{
    //    gvDiagnosis.EditIndex = -1;
    //    GetDiagnosisData();
    //    LoadSeqenceDescription();
    //    lblDeleteError.Text = "";
    //}
    //protected void gvDiagnosis_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    lblDeleteError.Text = "";
    //    string keyVal = gvDiagnosis.DataKeys[e.NewEditIndex].Value.ToString();

    //    //Find the DropDownList in the Row
    //    gvDiagnosis.EditIndex = e.NewEditIndex;
    //    GridViewRow editingRow = gvDiagnosis.Rows[e.NewEditIndex];
    //    if (ClaimType == CON.ClaimsType.Institutional)
    //    {
    //        DataSet ds = LoadDiagnosisData();

    //        var x = (from myrow in ds.Tables[0].AsEnumerable()
    //                 where myrow.Field<Int32>("Claims_Diagnosis_Information_ID") == Convert.ToInt32(keyVal)
    //                 select myrow.Field<Int32>("Claims_Sequence_Description_ID")).Take(1);

    //        String SelectedValue = Convert.ToString(x.FirstOrDefault());
    //        ViewState["SelectedSequenceDesc"] = SelectedValue;
    //    }
    //    GetDiagnosisData();
    //    LoadSeqenceDescription();
    //}
    //protected void gvDiagnosis_RowDeleting(object source, GridViewDeleteEventArgs e)
    //{
    //    lblDeleteError.Text = "";
    //    int keyVal = Convert.ToInt32(gvDiagnosis.DataKeys[e.RowIndex].Value);
    //    //int hospiceDocumentsByMailId = Convert.ToInt32(gvDiagnosis.DataKeyNames[e.RowIndex]["Claims_Diagnosis_Information_ID"].);

    //    DataSet dsServiceDetailsCheck = null;
    //    if(hdnClaimType_Diagnosis.Value != "1")
    //        dsServiceDetailsCheck = ClaimsController.CheckDiagnosisCodeUse(gvDiagnosis.DataKeys[e.RowIndex].Value.ToString(), Convert.ToInt32(hdnClaimIdDiagnosis.Value));
    //    if (!Helper.HasRows(dsServiceDetailsCheck) || hdnClaimType_Diagnosis.Value == "1")
    //    {
    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        try
    //        {
    //            svc.DeletePanelsData("Claims_Diagnosis_Information", "Claims_Diagnosis_Information_ID", keyVal);
    //        }
    //        catch (Exception ex) { }
    //    }
    //    else if (Helper.HasRows(dsServiceDetailsCheck))
    //    {
    //        lblDeleteError.Text = "Diagnosis code cannot be deleted since diagnosis pointer is used  in service detail for this diagnosis code.";
    //    }
    //        GetDiagnosisData();
    //        RefreshChildDropDown();
    //        LoadSeqenceDescription();
        
    //}
    #endregion

    #region 'dropdown'
    protected void DropdownBind_ICDVersions()
    {
        // binding ICD version 
        var dsICD = LookupTableController.GetICDVersions();
        var dtICD = dsICD.Tables[0];
        Helper.LoadList(ddlICDVer, dtICD, "ICD_Versions", "ICD_Versions", true);

        if (ddlICDVer.Items.FindByValue("ICD 10") != null)
        {
            ddlICDVer.ClearSelection();
            ddlICDVer.SelectedValue = "ICD 10";
            

        }
        lblIcdVersionError.Text = "";
    }
    protected void LoadSeqenceDescription()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_Id", hdnClaimIdDiagnosis.Value);
        DataSet dsSequenceDesc = svc.SelectPanelsData("claims_sequence_description", parms);

        if (Helper.HasRows(dsSequenceDesc))
        {
            Helper.LoadList(ddlDiagnosisSequenceDescription, dsSequenceDesc, "Sequence_Description", "Claims_Sequence_Description_ID", true);
        }

        //iterate and check the count of dropdown values
        if (ClaimType == CON.ClaimsType.Institutional)
        {
            DataSet ds = LoadDiagnosisData();
            int PrincipalCount = 0;
            int AdmittingCount = 0;
            int OtherCount = 0;
            int PatientReasonForVisitCount = 0;
            int ExternalCauseOfInjuryCount = 0;
            if (Helper.HasRows(ds))
            {


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(1))
                        {
                            PrincipalCount += 1;
                        }
                        else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(2))
                        {
                            AdmittingCount += 1;
                        }
                        else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(3))
                        {
                            OtherCount += 1;
                        }
                        else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(4))
                        {
                            PatientReasonForVisitCount += 1;
                        }
                        else if (Helper.GetInt("Claims_Sequence_Description_ID", dr).Equals(5))
                        {
                            ExternalCauseOfInjuryCount += 1;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            foreach (ListItem item in ddlDiagnosisSequenceDescription.Items)
            {
                if (item.Value == "1" && PrincipalCount >= 1)
                {
                    item.Enabled = false;
                }
                else if (item.Value == "2" && AdmittingCount >= 1)
                {
                    item.Enabled = false;
                }
                else if (item.Value == "4" && PatientReasonForVisitCount >= 3)
                {
                    item.Enabled = false;
                }
                else if (item.Value == "5" && ExternalCauseOfInjuryCount >= 12)
                {
                    item.Enabled = false;
                }
            }
        }
    }


    protected void LoadPresentOnAdmission()
    {
        ddlPresentAdmission.Items.Clear();
        ddlPresentAdmission.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        ddlPresentAdmission.Items.Insert(1, new ListItem("Yes", "Y"));
        ddlPresentAdmission.Items.Insert(2, new ListItem("No", "N"));
        ddlPresentAdmission.Items.Insert(3, new ListItem("Unknown", "U"));
        ddlPresentAdmission.Items.Insert(4, new ListItem("Not Applicable", "W"));
    }
    #endregion

    #region 'dropdown index changed events'
    protected void ddlDiagnosisSequenceDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiagnosisSequenceDescription.SelectedItem.Text == "Admitting" || ddlDiagnosisSequenceDescription.SelectedItem.Text == "Patient Reason for Visit")
        {
            ddlPresentAdmission.Enabled = false;
            lblDiagnosisDescription.Text = lblDiagDesc.Value;
        }
        else
        {
            ddlPresentAdmission.Enabled = true;
            lblDiagnosisDescription.Text = lblDiagDesc.Value;
        }
    }

    protected void dropDownSeqDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList drpDownSequenceDescription;
        DropDownList drpDownPresentOnAdmission;
        //foreach (GridViewRow row in gvDiagnosis.Rows)
        //{
        //    drpDownSequenceDescription = row.FindControl("dropDownSeqDesc") as DropDownList;
        //    drpDownPresentOnAdmission = row.FindControl("drpdownPresentOnAdmission") as DropDownList;
        //    if (drpDownSequenceDescription != null && drpDownPresentOnAdmission != null)
        //    {
        //        if (drpDownSequenceDescription.SelectedItem.Text == "Admitting" || drpDownSequenceDescription.SelectedItem.Text == "Patient Reason for Visit")
        //        {
        //            drpDownPresentOnAdmission.Enabled = false;
        //        }
        //        else
        //        {
        //            drpDownPresentOnAdmission.Enabled = true;
        //        }
        //    }
        //}
    }

    protected void drpdownIcdVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblIcdVersionError.Text = "";
        TextBox txtBoxDiagnosisCode;
        DropDownList drpDownICDVersion;
        Label lblGridDiagnosisDescription;
        //foreach (GridViewRow row in gvDiagnosis.Rows)
        //{
        //    txtBoxDiagnosisCode = row.FindControl("txtGridDiagnosisCode") as TextBox;
        //    drpDownICDVersion = row.FindControl("drpdownIcdVersion") as DropDownList;
        //    lblGridDiagnosisDescription = row.FindControl("lblEditDiagnosisDescription") as Label;
        //    if (drpDownICDVersion != null)
        //    {
        //        if (!string.IsNullOrEmpty(txtBoxDiagnosisCode.Text))
        //        {
        //            var dsICD = LookupTableController.GetICDDiagnosisCode(txtBoxDiagnosisCode.Text.Trim(), drpDownICDVersion.SelectedValue, "");
        //            if (dsICD != null && Helper.HasRows(dsICD))
        //            {
        //                var dtICD = dsICD.Tables[0];
        //                dtICD = dtICD.Select("ICD10Diag <> ''").CopyToDataTable();
        //                if (dtICD.Rows.Count > 0)
        //                {
        //                    DataRow r = dtICD.Rows[0];
        //                    //ddlICDVer.SelectedValue = ddlICDVer.SelectedValue;
        //                    lblGridDiagnosisDescription.Text = r["DiagDesc"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                lblIcdVersionError.Text = "Incorrect Diagnosis Code";
        //                lblGridDiagnosisDescription.Text = "";
        //            }
        //        }
        //        else
        //        {
        //            ddlICDVer.SelectedIndex = 0;
        //        }
        //    }
        //}
    }

    protected void txtGridDiagnosisCode_textChanged(object sender, EventArgs e)
    {
        lblIcdVersionError.Text = "";
        TextBox txtBoxDiagnosisCode;
        DropDownList drpDownICDVersion;
        Label lblGridDiagnosisDescription;
        //foreach (GridViewRow row in gvDiagnosis.Rows)
        //{
        //    txtBoxDiagnosisCode = row.FindControl("txtGridDiagnosisCode") as TextBox;
        //    drpDownICDVersion = row.FindControl("drpdownIcdVersion") as DropDownList;
        //    lblGridDiagnosisDescription = row.FindControl("lblEditDiagnosisDescription") as Label;
        //    if (txtBoxDiagnosisCode != null)
        //    {
        //        if (!string.IsNullOrEmpty(txtBoxDiagnosisCode.Text))
        //        {
        //            var dsICD = LookupTableController.GetICDDiagnosisCode(txtBoxDiagnosisCode.Text.Trim(), drpDownICDVersion.SelectedValue, "");
        //            if (dsICD != null && Helper.HasRows(dsICD))
        //            {
        //                var dtICD = dsICD.Tables[0];
        //                dtICD = dtICD.Select("ICD10Diag <> ''").CopyToDataTable();
        //                if (dtICD.Rows.Count > 0)
        //                {
        //                    DataRow r = dtICD.Rows[0];
        //                    //ddlICDVer.SelectedValue = ddlICDVer.SelectedValue;
        //                    lblGridDiagnosisDescription.Text = r["DiagDesc"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                lblIcdVersionError.Text = "Incorrect Diagnosis Code";
        //                lblGridDiagnosisDescription.Text = "";
        //            }
        //        }
        //        else
        //        {
        //            ddlICDVer.SelectedIndex = 0;
        //        }
        //    }
        //}
    }
    protected void txtDiagnosisCode_TextChanged(object sender, EventArgs e)
    {
        ValidateDiagnosisCode();
    }

    protected void ddlICDVer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ValidateDiagnosisCode();
    }


    protected void drpdownPresentOnAdmission_SelectedOndexChanged(object sender, EventArgs e)
    {

    }
    #endregion



    private void ValidateDiagnosisCode()
    {
        lblIcdVersionError.Text = "";
        if (!string.IsNullOrEmpty(txtDiagnosisCode.Text))
        {
            var dsICD = LookupTableController.GetICDDiagnosisCode(txtDiagnosisCode.Text.Trim(), ddlICDVer.SelectedValue, "");
            if (dsICD != null && Helper.HasRows(dsICD))
            {
                var dtICD = dsICD.Tables[0];
                dtICD = dtICD.Select("ICD10Diag <> ''").CopyToDataTable();
                if (dtICD.Rows.Count > 0)
                {
                    DataRow row = dtICD.Rows[0];
                    ddlICDVer.SelectedValue = ddlICDVer.SelectedValue;
                    lblDiagnosisDescription.Text = row["DiagDesc"].ToString();
                }
            }
            else
            {
                lblIcdVersionError.Text = "Incorrect Diagnosis Code";
                lblDiagnosisDescription.Text = "";
            }
        }
        else
        {
            ddlICDVer.SelectedIndex = 0;
        }
    }



    private void SetDiagnosisPanelData(List<Diagnosis> diagnoses)
    {
        ClaimService.Diagnoses = diagnoses;
      //  gvDiagnosis.DataSource = diagnoses;
      //  gvDiagnosis.DataBind();
    }

    private DataSet LoadDiagnosisData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaimIdDiagnosis.Value.ToString());
        parms.Add("Claim_Type", hdnClaimType_Diagnosis.Value.ToString());
        try
        {
            dsDiagnosisCodes = svc.SelectPanelsData("Claims_Diagnosis_Information", parms);
        }
        catch (Exception ex) { }
        return dsDiagnosisCodes;
    }

    #region 'Visibility'
    private void SetFieldsVisibility(string claimTypeId)
    {
        if (claimTypeId == CON.ClaimsType.Institutional)
        {
            divSequencedescription.Visible = divPresetOnAdmission.Visible = true;
            btnICDVersionSearch.Visible = false;
            //foreach (DataControlField column in gvDiagnosis.Columns)
            //{

            //    if (column.HeaderText == "Sequence" || column.HeaderText == "Present On Admission")
            //    {
            //        column.Visible = true;
            //    }
            //    if (column.HeaderText == "Sequence ")
            //    {
            //        column.Visible = false;
            //    }
            //}
        }

        else
        {
            divSequencedescription.Visible = divPresetOnAdmission.Visible = false;
            btnICDVersionSearch.Visible = true;
            //foreach (DataControlField column in gvDiagnosis.Columns)
            //{
            //    if (column.HeaderText == "Sequence ")
            //    {
            //        column.Visible = true;
       //     //    }

            //    if (column.HeaderText == "Sequence" || column.HeaderText == "Present On Admission")
            //    {
            //        column.Visible = false;
            //    }
            //}
        }

       

       // GetDiagnosisData();
        DropdownBind_ICDVersions();
        LoadSeqenceDescription();
        //LoadPresentOnAdmission();        
        RefreshChildDropDown();
    }
    private void SetFieldsVisibilityClaimtype(string claimTypeId)
    {
        if (claimTypeId == CON.ClaimsType.Institutional)
        {
            divSequencedescription.Visible = divPresetOnAdmission.Visible = true;
            btnICDVersionSearch.Visible = false;

        }

        else
        {
            divSequencedescription.Visible = divPresetOnAdmission.Visible = false;
            btnICDVersionSearch.Visible = true;

        }


    }
    #endregion

    #region 'Setter Invoked Method'
    private void GetDiagCode(DataSet dataSet)
    {
        dsDiagCode = new DataSet();


        if (!string.IsNullOrEmpty(hdnClaimIdDiagnosis.Value) || !string.IsNullOrEmpty(ICNNumber))
        {
            if (Helper.HasRows(dataSet))
            {
                dsDiagCode = dataSet;
            }
            else
            {
                 dsDiagCode = LoadDiagnosisData();
            }
        }
        if (Helper.HasRows(dsDiagCode))
        {
            
            DataTable diagTable = dsDiagCode.Tables[0];
            if ((hdnClaimType_Diagnosis.Value == "0")|| hdnClaimType_Diagnosis.Value == "2") {
              
                providerDiagOutput.InnerHtml = "";
                if (diagTable.Rows.Count > 0)
                {
                    string tab = string.Empty;
                     tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
               
                    foreach (DataRow dr in diagTable.Rows)
                    {

                        string sequence = dr["diag_seq"].ToString();
                        string DiagnosisCode = dr["diag_code"].ToString();
                        string ICDVersion = dr["ICDVersion"].ToString();
                        string Claims_Diagnosis_Information_ID = dr["Claims_Diagnosis_Information_ID"].ToString();

                        string DiagnosisCodeDescription = dr["diagnosisDescription"].ToString();
                        if(string.IsNullOrEmpty(DiagnosisCodeDescription))
                        {
                            var dsICD = LookupTableController.GetICDDiagnosisCode(DiagnosisCode, ICDVersion, "");
                            if (dsICD != null && Helper.HasRows(dsICD))
                            {
                                var dtICD = dsICD.Tables[0];
                                dtICD = dtICD.Select("ICD10Diag <> ''").CopyToDataTable();
                                if (dtICD.Rows.Count > 0)
                                {
                                    DataRow row = dtICD.Rows[0];
                                    DiagnosisCodeDescription = row["DiagDesc"].ToString();
                                }
                            }
                        }
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DiagnosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_Diagnosis_Information_ID + "\",\"" + hdnClaimIdDiagnosis.Value + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_Diagnosis_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                            else
                            {
                                tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DiagnosisCodeDescription + "</td></tr >";
                            }
                        }
                    }
                    tab = tab + "</tbody></table>";
                    providerDiagOutput.InnerHtml = tab;
                }
               
            }

           else if (hdnClaimType_Diagnosis.Value == "1")
            { 
                providerDiagOutput.InnerHtml = "";
                if (diagTable.Rows.Count > 0)
                {
                    string tab = string.Empty;
                    tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Present On Admission</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                    foreach (DataRow dr in diagTable.Rows)
                    {

                        string sequence = dr["seq_Desc"].ToString();
                        string DiagnosisCode = dr["diag_code"].ToString();
                        string ICDVersion = dr["ICDVersion"].ToString();
                        string PresentOnAdmission = "";
                        if (dr["Present_On_Admission"].ToString().Equals("W"))
                        {
                            PresentOnAdmission = "Not Applicable";
                        } else if (dr["Present_On_Admission"].ToString().Equals("N"))
                        {
                            PresentOnAdmission = "No";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("Y"))
                        {
                            PresentOnAdmission = "Yes";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("U"))
                        {
                            PresentOnAdmission = "Unknown";
                        } else
                        {
                            PresentOnAdmission = dr["Present_On_Admission"].ToString();
                        }
                        string DiagnosisCodeDescription = dr["diagnosisDescription"].ToString();
                        string Claims_Diagnosis_Information_ID= dr["Claims_Diagnosis_Information_ID"].ToString();
                        if (string.IsNullOrEmpty(DiagnosisCodeDescription))
                        {
                            var dsICD = LookupTableController.GetICDDiagnosisCode(DiagnosisCode, ICDVersion, "");
                            if (dsICD != null && Helper.HasRows(dsICD))
                            {
                                var dtICD = dsICD.Tables[0];
                                dtICD = dtICD.Select("ICD10Diag <> ''").CopyToDataTable();
                                if (dtICD.Rows.Count > 0)
                                {
                                    DataRow row = dtICD.Rows[0];
                                    DiagnosisCodeDescription = row["DiagDesc"].ToString();
                                }
                            }
                        }
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DiagnosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_Diagnosis_Information_ID + "\",\"" + hdnClaimIdDiagnosis.Value + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_Diagnosis_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            }
                            else
                            {
                                tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DiagnosisCodeDescription + "</td></tr >";
                            }
                        }
                    }
                    tab = tab + "</tbody></table>";
                    providerDiagOutput.InnerHtml = tab;
                }

            }
        }
        else
        {

            //gvDiagnosis.DataSource = null;
            //gvDiagnosis.DataBind();
            providerDiagOutput.InnerHtml = "";
        }
    }
    #endregion

    #region 'Clear Fields'
    public void ClearDiagnosisCodeFields()
    {
        lblDiagnosisDescription.Text = "";
        txtDiagnosisCode.Text = "";
        ddlDiagnosisSequenceDescription.ClearSelection();
        ddlPresentAdmission.ClearSelection();
        ddlICDVer.ClearSelection();
        dsDiagCode.Clear();
        providerDiagOutput.InnerHtml = "";



    }

    public void ClearGrid()
    {
        //gvDiagnosis.DataSource = null;
        //gvDiagnosis.DataBind();
        providerDiagOutput.InnerHtml = "";
    }
    #endregion

    #region 'Invoked By DisplayreadOnly'
    private void BindGrid()
    {
        //VISIBILITY
        if (DisplayReadOnly)
        {
            btnDiagnosis.Visible = !DisplayReadOnly;
            divDiagnosisCode.Visible = !DisplayReadOnly;
            divICDVersion.Visible = !DisplayReadOnly;
            divPresetOnAdmission.Visible = !DisplayReadOnly;
            divBtndiagnosis.Visible = !DisplayReadOnly;
            divSequencedescription.Visible = !DisplayReadOnly;
            divDiagnosisDescription.Visible = !DisplayReadOnly;
            ddlDiagnosisSequenceDescription.Visible = !DisplayReadOnly;
            Seqdiv.Visible = !DisplayReadOnly;
        }
        else
        {
            btnDiagnosis.Visible = !DisplayReadOnly;
            divDiagnosisCode.Visible = !DisplayReadOnly;
            divICDVersion.Visible = !DisplayReadOnly;
            divPresetOnAdmission.Visible = !DisplayReadOnly;
            divBtndiagnosis.Visible = !DisplayReadOnly;
            divSequencedescription.Visible = !DisplayReadOnly;
            divDiagnosisDescription.Visible = !DisplayReadOnly;
            ddlDiagnosisSequenceDescription.Visible = !DisplayReadOnly;
            Seqdiv.Visible = !DisplayReadOnly;

        }

        //DATABIND
        //if (Helper.HasRows(dsDiagnosisCodes))
        //{
        //    gvDiagnosis.DataSource = dsDiagnosisCodes;
        //    gvDiagnosis.DataBind();
        //}
        //else
        //{
        //    gvDiagnosis.DataSource = null;
        //    gvDiagnosis.DataBind();
        //}
    }
    #endregion

    public void SaveToDbonAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string line = "";
                int rows = i;               
                line = (rows + 1).ToString();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_ID", hdnClaimIdDiagnosis.Value);
              //  parms.Add("Claim_Type", hdnClaimType_Diagnosis.Value);    
                parms.Add("Sequence", line);
                parms.Add("Diagnosis_Code", dt.Rows[i]["diag_code"].ToString());
                parms.Add("ICD_Version", dt.Rows[i]["ICDVersion"].ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                if (hdnClaimType_Diagnosis.Value == CON.ClaimsType.Institutional)
                {
                    if(dt.Rows[i]["seq_Desc"].ToString().ToUpper() == "PRINCIPAL")
                    { parms.Add("Sequence_Description_ID","1"); }                   
                    if (dt.Rows[i]["seq_Desc"].ToString().ToUpper() == "ADMITTING")
                    { parms.Add("Sequence_Description_ID", "2"); }
                    if (dt.Rows[i]["seq_Desc"].ToString().ToUpper() == "OTHER")
                    { parms.Add("Sequence_Description_ID", "3"); }
                    if (dt.Rows[i]["seq_Desc"].ToString().ToUpper() == "PATIENT REASON FOR VISIT")
                    { parms.Add("Sequence_Description_ID", "4"); }
                    if (dt.Rows[i]["seq_Desc"].ToString().ToUpper() == "EXTERNAL CAUSE OF INJURY")
                    { parms.Add("Sequence_Description_ID", "5"); }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Present_On_Admission"].ToString()))
                    {
                        if (dt.Rows[i]["Present_On_Admission"].ToString().Equals("W"))
                        {
                            parms.Add("Present_On_Admission", "Not Applicable");
                        }
                        else if (dt.Rows[i]["Present_On_Admission"].ToString().Equals("N"))
                        {
                            parms.Add("Present_On_Admission", "No");
                        }
                        else if (dt.Rows[i]["Present_On_Admission"].ToString().Equals("Y"))
                        {
                            parms.Add("Present_On_Admission", "Yes");
                        }
                        else if (dt.Rows[i]["Present_On_Admission"].ToString().Equals("U"))
                        {
                            parms.Add("Present_On_Admission", "Unknown");
                        }
                        else
                        {
                            parms.Add("Present_On_Admission", dt.Rows[i]["Present_On_Admission"].ToString());
                        }
                    }
                    else
                        parms.Add("Present_On_Admission", string.Empty);
                }
                else
                {
                    parms.Add("Sequence_Description_ID", null);
                    parms.Add("Present_On_Admission", null);
                }
                try
                {
                    svc.InsertPanelsData("Claims_Diagnosis_Information", parms);
                }
                catch (Exception ex) { }
            }
        }     
        LoadSeqenceDescription();        
        GetDiagCode(null);

    }

}