using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PopupControls_ICDProcedureCode : System.Web.UI.UserControl
{

    #region "property"
    private DataSet dsICDProducerCodeInformation = new DataSet();
    public string IcnNumber = string.Empty;
    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(IcnNumber))
                return IcnNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                IcnNumber = value.Trim();
        }
    }
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimIDInstitutional.Value))
                return hdnClaimIDInstitutional.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimIDInstitutional.Value = value.Trim();
        }
    }
    public DataSet dataSetICDProdureCodeInfo
    {
        get
        {
            if (Helper.HasRows(FetchServiceInformationInformation()))
            {
                return dsICDProducerCodeInformation;
            }
            else
            {
                return dsICDProducerCodeInformation;
            }
        }

        set
        {
            if (value != null)
            {
                dsICDProducerCodeInformation = value;
                SetICDProcedureCodes(value);
                ViewState["ICDCodes"] = value;
                //GetICDProcedureCodeData(value);
            }
        }
    }
    public string Sequence
    {
        get { return ddlSequenceICD.SelectedValue; }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlSequenceICD.Items.FindByValue(value.ToString().Trim()) != null)
            {
                ddlSequenceICD.ClearSelection();
                ddlSequenceICD.SelectedValue = value.Trim();
            }
        }
    }
    public string SequenceDisplay
    {
        get
        {
            if (ddlSequenceICD.SelectedItem != null && !string.IsNullOrEmpty(ddlSequenceICD.SelectedItem.Text))
                return ddlSequenceICD.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlSequenceICD.Items.FindByText(value.ToString().Trim()) != null)
            {
                ddlSequenceICD.ClearSelection();
                ddlSequenceICD.SelectedItem.Text = value.Trim();
            }
        }
    }
    public string ICDProcedureCodeDescription
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtICD10ProcedureCodeDec.Value))
                return txtICD10ProcedureCodeDec.Value;

            else
                return string.Empty;
        }
        set { txtICD10ProcedureCodeDec.Value = value; }
    }


    public string Date
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtICDPrCdDate.Text))
                return txtICDPrCdDate.Text;

            else
                return string.Empty;
        }
        set
        { txtICDPrCdDate.Text = value; }
    }
    public string ICDVersion
    {
        get
        {
            if (ddlICDPrCodeInstiutional.SelectedItem != null && !string.IsNullOrEmpty(ddlICDPrCodeInstiutional.SelectedValue))
                return ddlICDPrCodeInstiutional.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlICDPrCodeInstiutional.Items.FindByValue(value.ToString().Trim()) != null)
            {
                ddlICDPrCodeInstiutional.ClearSelection();
                ddlICDPrCodeInstiutional.SelectedValue = value.Trim();
            }
        }
    }
    public string ICDVersionDisplay
    {
        get
        {
            if (ddlICDPrCodeInstiutional.SelectedItem != null && !string.IsNullOrEmpty(ddlICDPrCodeInstiutional.SelectedItem.Text))
                return ddlICDPrCodeInstiutional.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlICDPrCodeInstiutional.Items.FindByText(value.ToString().Trim()) != null)
            {
                ddlICDPrCodeInstiutional.ClearSelection();
                ddlICDPrCodeInstiutional.SelectedItem.Text = value.Trim();
            }
        }
    }

    public string ICDProcedureCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtICD10Procedurecode.Text))
                return txtICD10Procedurecode.Text;

            else
                return string.Empty;
        }
        set { txtICD10Procedurecode.Text = value; }
    }
    private bool _displayReadOnly;
    public bool DisplayReadOnly
    {
        get
        {


            return _displayReadOnly;
        }
        set
        {
            _displayReadOnly = value;
            //GetICDProcedureCodeData(null);
            SetButtonVisibility();

        }
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            divICD.Visible = false;
        }
        else
        {
            divICD.Visible = true;
        }
    }
    #endregion
    private DataSet FetchServiceInformationInformation()
    {
        DataSet dataSetICDProdureCodeInfo = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaimIDInstitutional.Value.ToString());
        dsICDProducerCodeInformation = dataSetICDProdureCodeInfo = spa.SelectPanelsData("claims_icd_procedureCode", parms);
        return dataSetICDProdureCodeInfo;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetSequenceType();
            GetICDProcedureCodeData(null);
        }
        else
        {
            //  btnAddICD10ProcedureCode.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnAddICD10ProcedureCode, null) + ";");
        }
        if (DisplayReadOnly == true)
        {
            divICD.Visible = false;
        }
        else
        { divICD.Visible = true; }
        if (ICDProcedureOutput.Visible == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displaytableIcdProc();</script>", false);
        }


    }
    public bool CanUserViewDelete()
    {
        return true;
    }
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
    public string Claim_Id { get; set; }
    public string prCode { get; set; }
    protected void grdICD10ProcedureCode_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #region spa
    private PDMSService.PDMSServiceClient _spa;
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion
    private void GetSequenceType()
    {
        ddlSequenceICD.Items.Clear();
        DataSet dataSet = spa.GetSequenceType();
        DataTable dt = dataSet.Tables[0];
        var row_Sequence = from row in dt.AsEnumerable()
                           where row.Field<int>("PRIOR_AUTH_CLAIM_SEQUENCE_ID") == 2 || row.Field<int>("PRIOR_AUTH_CLAIM_SEQUENCE_ID") == 3

                           select row;
        DataTable dt_Sequence = row_Sequence.CopyToDataTable();
        Helper.LoadList(ddlSequenceICD, dt_Sequence, "PRIOR_AUTH_CLAIM_SEQUENCE_DESC", "PRIOR_AUTH_CLAIM_SEQUENCE_ID", true);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_ID))
        {
            txtICD10Procedurecode.Text = ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_ID;
            txtICD10ProcedureCodeDec.Value = ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_Description;
            txtICD10ProcedureCodeDec1.Text = ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_Description;
            //if (gvICD10ProcedureCode.Rows.Count > 0)
            //{
            //    for (int i = 0; i < gvICD10ProcedureCode.Rows.Count; i++)
            //    {

            //        TextBox GridProcedureCodeTextbox = (TextBox)gvICD10ProcedureCode.Rows[i].FindControl("txtICD10GVProcedurecodeInstitutional");
            //        Label lblICDDescription = (Label)gvICD10ProcedureCode.Rows[i].FindControl("lblICDDescription");
            //        HiddenField lblICDDescription1 = (HiddenField)gvICD10ProcedureCode.Rows[i].FindControl("lblICDDescription1");
            //        if (GridProcedureCodeTextbox != null && lblICDDescription != null)
            //        {
            //            GridProcedureCodeTextbox.Text = ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_ID;
            //            lblICDDescription.Text = ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_Description;
            //            lblICDDescription1.Value = ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_Description;
            //            txtICD10Procedurecode.Text = "";
            //            txtICD10ProcedureCodeDec.Value = "";
            //            txtICD10ProcedureCodeDec1.Text = "";
            //        }

            //    }
            //}

            if (!string.IsNullOrEmpty(txtICD10Procedurecode.Text))
            {

                ucSubmitClaimICD10ProcedureCodes.clearhiddendata();

            }

        }
    }


    protected void lnkICD10GVProcedurecodeInstitutional_Click(object sender, EventArgs e)
    {

        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        mpeSubmitClaimSearchPop.Show();



    }

    #region ICD Procedure Code Institutional
   
    protected void ReportProcedureDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtICDPrCdDate.Text, true) && Helper.IsValidDate(txtICDPrCdDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtICDPrCdDate.Text).Subtract(Convert.ToDateTime(txtICDPrCdDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    public void GetICDProcedureCodeData(DataSet dsOtherPayerInfo)
    {
        if (!string.IsNullOrEmpty(hdnClaimIDInstitutional.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimIDInstitutional.Value);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimIDInstitutional.Value, true));
            int sequencePrincipalCount = 0;
            int sequenceOtherCount = 0;
            DataSet dtSequenceDetails = DataAccess.ExecuteStoredProcedure("usp_Claims_ICD_Procedure_Code_Sequence_TotalSequenceCount", parameters, "Claims_ICD_Procedure_Code_Sequence");
            if (Helper.HasRows(dtSequenceDetails))
            {
                var sequencePrincipaldata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "2").ToList();
                var sequenceotherdata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "3").ToList();

                if (sequencePrincipaldata.Count() != 0)
                {
                    sequencePrincipalCount = Convert.ToInt32(sequencePrincipaldata.FirstOrDefault().ItemArray[0]);
                }
                else
                {

                }
                if (sequenceotherdata.Count() != 0)
                {
                    sequenceOtherCount = Convert.ToInt32(sequenceotherdata.ToList().FirstOrDefault().ItemArray[0]);
                    if (sequenceOtherCount == 24)
                    {
                        //      btnAddICDCode.Visible = false;
                    }
                    else
                    {
                        // btnAddICDCode.Visible = true;

                    }
                }
            }
            DataSet dsICDProcedureCode = ClaimsController.SelectPanelsData("claims_icd_procedureCode", parms);
            if (Helper.HasRows(dsICDProcedureCode))
            {
                DataTable dtICDProcedureData = dsICDProcedureCode.Tables[0];
                if (dtICDProcedureData.Rows.Count > 0)
                {
                    string table = string.Empty;
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Icd Procedure Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Date</th><th style='width:30px; scope='col'>ICD Procedure Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                    foreach (DataRow dr in dtICDProcedureData.Rows)
                    {
                        string Claims_ICD_Procedure_Code_Sequence_ID = dr["Claims_ICD_Procedure_Code_Sequence_ID"].ToString();
                        string Sequence = dr["Sequence_No"].ToString();
                        if (Sequence == "2")
                        {
                            Sequence = "Other";
                        }
                        else if (Sequence == "3")
                        {
                            Sequence = "Principal";
                        }
                        string IcdProcedureCode = dr["ICD_Procedure_Code"].ToString();
                        string ICDVersion = dr["ICD_Version"].ToString();
                        string IcdProcedureCodeDescription = dr["Procedure_Code_Description"].ToString();
                        string Claim_ID = dr["Claim_ID"].ToString();
                        string Date =Convert.ToDateTime(dr["ICD_Date"]).ToString("MM/dd/yyyy");
                        string Perior_Auth_Claim_Sequence_Desc = dr["PRIOR_AUTH_CLAIM_SEQUENCE_Desc"].ToString();

                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Sequence + "</span></td><td><span title='Line' class='tNumber'>" + IcdProcedureCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + Date + "</span></td><td>" + IcdProcedureCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditICDProcdureCodeLineItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\",\"" + Claim_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteICDProcedureCodeItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";


                    }
                    table = table + "</tbody></table>";
                    ICDProcedureOutput.InnerHtml = table;

                }

            }
            else
            {
                ICDProcedureOutput.InnerHtml = "";
            }
        }
        if (DisplayReadOnly == true)
        {
            divICD.Visible = false;
        }

        else
        {
            divICD.Visible = true;
        }

    }


    private void SetICDProcedureCodes(DataSet dsICDProcedureCodes)

    {
        //gvICD10ProcedureCode.DataSource = dsICDProcedureCodes;
        //gvICD10ProcedureCode.DataBind();
        try
        {
            hdnICDProcedureCodeClaimStatus.Value = "";
            //DataSet dsICDProcedureCodeDetails = new DataSet();
            if (!string.IsNullOrEmpty(hdnClaimIDInstitutional.Value))
            {
                if (Helper.HasRows(dsICDProcedureCodes))
                {
                    dsICDProducerCodeInformation = dsICDProcedureCodes;
                }
                else
                {
                    dsICDProducerCodeInformation = FetchServiceInformationInformation();
                }
            }
            if (Helper.HasRows(dsICDProducerCodeInformation))
            {
                DataTable ICDTable = dsICDProducerCodeInformation.Tables[0];
                if (ICDTable.Rows.Count > 0)
                {
                    string tab = string.Empty;
                    tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Icd Procedure Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Date</th><th style='width:30px; scope='col'>ICD Procedure Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    foreach (DataRow dr in ICDTable.Rows)
                    {
                        var Sequence = dr["Sequence_No"].ToString();
                        if (Sequence == "2")
                        {
                            Sequence = "Other";
                        }
                        else if (Sequence == "3")
                        {
                            Sequence = "Principal";
                        }

                        var IcdProcedureCode = dr["ICD_Procedure_Code"].ToString();
                        var ICDVersion = dr["ICD_Version"].ToString();
                        var IcdProcedureCodeDescription = dr["Procedure_Code_Description"].ToString();
                        var Claims_ICD_Procedure_Code_Sequence_ID = dr["Claims_ICD_Procedure_Code_Sequence_ID"].ToString();
                        var Claim_ID = dr["Claim_ID"].ToString();
                        var ICD_Date = Convert.ToDateTime(dr["ICD_Date"]).ToString("MM/dd/yyyy");

                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Sequence + "</span></td><td><span title='Line' class='tNumber'>" + IcdProcedureCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + ICD_Date + "</span></td><td>" + IcdProcedureCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditICDProcdureCodeLineItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\",\"" + Claim_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteICDProcedureCodeItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            hdnICDProcedureCodeClaimStatus.Value = "Pending Submission";
                        }
                        else
                        {
                            tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Sequence + "</span></td><td><span title='Line' class='tNumber'>" + IcdProcedureCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + ICD_Date + "</span></td><td>" + IcdProcedureCodeDescription + " </td></tr >";
                            hdnICDProcedureCodeClaimStatus.Value = "Other";
                        }
                    }
                    tab = tab + "</tbody></table>";
                    ICDProcedureOutput.InnerHtml = tab;

                }
                else {
                    ICDProcedureOutput.InnerHtml = "";

                }
            }
        }
        catch (Exception ex) { }
    }

        protected void gvICD10ProcedureCode_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header && DisplayReadOnly)
        {
            e.Row.Cells[5].Visible = !DisplayReadOnly;
        }
        if (e.Row.RowType == DataControlRowType.DataRow && DisplayReadOnly)
        {

            Button btEdit = (Button)e.Row.Cells[5].FindControl("btnEdit");
            Button btDelete = (Button)e.Row.Cells[5].FindControl("btnDelete");
            if (btEdit != null)
                btEdit.Visible = !DisplayReadOnly;
            if (btDelete != null)
                btDelete.Visible = !DisplayReadOnly;

        }
        if (e.Row.RowType == DataControlRowType.DataRow &&
        (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
        {
            //Find the DropDownList in the Row
            DropDownList drpdownIcdVersion = (e.Row.FindControl("ddlGVSequenceInstitutional") as DropDownList);
            DataSet dt_Sequence = LookupTableController.GetSequenceType();
            DataTable dtSequenceDetails = dt_Sequence.Tables[0];
            var row_Sequence = from row in dtSequenceDetails.AsEnumerable()
                               where row.Field<int>("PRIOR_AUTH_CLAIM_SEQUENCE_ID") == 2 || row.Field<int>("PRIOR_AUTH_CLAIM_SEQUENCE_ID") == 3

                               select row;
            DataTable dt_SequenceCopy = row_Sequence.CopyToDataTable();
            Helper.LoadList(drpdownIcdVersion, dt_SequenceCopy, "PRIOR_AUTH_CLAIM_SEQUENCE_DESC", "PRIOR_AUTH_CLAIM_SEQUENCE_ID", true);
        }
    }
    protected void gvICD10ProcedureCode_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        //string keyVal = gvICD10ProcedureCode.DataKeys[e.RowIndex].Value.ToString();
        //DropDownList sequenceInstitutional = (DropDownList)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("ddlGVSequenceInstitutional");
        //HiddenField hdnSequenceInstitutional = (HiddenField)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("hdnSequenceInstitutional");
        //HiddenField hdnICDPrCd10Institutional = (HiddenField)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("hdnICDPrCd10Institutional");
        //TextBox procedureCodeInstitutional = (TextBox)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("txtICD10GVProcedurecodeInstitutional");
        //DropDownList icdVersionDdl = (DropDownList)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("ddlGVICDVersionInstitutional");
        //Label lblgrvError = (Label)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("lblgrvError");
        //TextBox txtICDGVDateInstitutional = (TextBox)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("txtICDGVDateInstitutional");
        //HiddenField lblICDDescription = (HiddenField)gvICD10ProcedureCode.Rows[e.RowIndex].FindControl("lblICDDescription1");
        //string GridProcedureCode = string.Empty;

        //GridProcedureCode = string.IsNullOrEmpty(ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_ID) ?
        //procedureCodeInstitutional.Text : ucSubmitClaimICD10ProcedureCodes.ICD_PROCEDURE_CODE_ID;



        //if (string.IsNullOrEmpty(GridProcedureCode) && hdnICDPrCd10Institutional != null)
        //{
        //    GridProcedureCode = hdnICDPrCd10Institutional.Value;
        //}
        //Updating db
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_Id", hdnClaimIDInstitutional.Value);
        DataSet dsDiagnosis = spa.SelectPanelsData("claims_icd_procedureCode", parms);
        if (!string.IsNullOrEmpty(hdnClaimIDInstitutional.Value))
        {

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimIDInstitutional.Value, true));
            int sequencePrincipalCount = 0;
            int sequenceOtherCount = 0;
            DataSet dtSequenceDetails = DataAccess.ExecuteStoredProcedure("usp_Claims_ICD_Procedure_Code_Sequence_TotalSequenceCount", parameters, "Claims_ICD_Procedure_Code_Sequence");
            if (Helper.HasRows(dtSequenceDetails))
            {
                var sequencePrincipaldata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "3").ToList();
                var sequenceotherdata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "2").ToList();

                if (sequencePrincipaldata.Count() != 0)
                {
                    sequencePrincipalCount = Convert.ToInt32(sequencePrincipaldata.FirstOrDefault().ItemArray[0]);
                }

                if (sequenceotherdata.Count() != 0)
                {
                    sequenceOtherCount = Convert.ToInt32(sequenceotherdata.ToList().FirstOrDefault().ItemArray[0]);
                }
            }
            //if ((sequencePrincipalCount >= 1) && (sequenceInstitutional.SelectedValue == "3") && (!string.IsNullOrEmpty(sequenceInstitutional.SelectedValue)))
            //{
            //    lblgrvError.Visible = true;
            //    lblgrvError.Text = "Already principal sequence is added";
            //}
            //else
            {
                //  lblgrvError.Visible = false;
                //parms.Add("Claims_ICD_Procedure_Code_Sequence_ID", keyVal);
                //if (!string.IsNullOrEmpty(sequenceInstitutional.SelectedValue))
                //{
                //    parms.Add("Sequence_NO", sequenceInstitutional.SelectedValue);
                //}
                //else if (hdnSequenceInstitutional != null)
                //{
                //    parms.Add("Sequence_NO", hdnSequenceInstitutional.Value);

                //}
                //parms.Add("ICD_Version", string.IsNullOrEmpty(icdVersionDdl.SelectedValue) ? null : icdVersionDdl.SelectedValue);
                //parms.Add("ICD_Procedure_Code", GridProcedureCode);
                //parms.Add("Procedure_Code_Description", string.IsNullOrEmpty(lblICDDescription.Value) ? null : lblICDDescription.Value);

                //if (e.NewValues["Procedure_Code_Description"] != null)
                //{
                //    parms.Add("Procedure_Code_Description", e.NewValues["Procedure_Code_Description"].ToString());
                //}
                //else
                //{
                //    parms.Add("Procedure_Code_Description", null);

                //}

                //parms.Add("ICD_DATE", txtICDGVDateInstitutional.Text);

                //parms.Add("LAST_MODIFIED_DATE", DateTime.Now.ToString());
                //parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                //spa.UpdatePanelsData("claims_icd_procedureCode", parms);
                //gvICD10ProcedureCode.EditIndex = -1;

                //GetICDProcedureCodeData(null);
                //ucSubmitClaimICD10ProcedureCodes.clearhiddendata();
            }
        }
        //Displaying binded data from db 

    }

    
    public void ClearICDPrCodePanel()
    {
        
        ddlSequenceICD.SelectedIndex = 0;
        txtICD10Procedurecode.Text = "";
        ddlICDPrCodeInstiutional.SelectedIndex = 0;
        txtICDPrCdDate.Text = "";
        txtICD10ProcedureCodeDec.Value = "";
        txtICD10ProcedureCodeDec1.Text = "";
        ICDProcedureOutput.InnerHtml = "";
    }


    public void ClearGrid()
    {
        //gvICD10ProcedureCode.DataSource = null;
        //gvICD10ProcedureCode.DataBind();
    }
    #endregion



    protected void lnkICD10Procedurecode_Click(object sender, EventArgs e)
    {
        lblError.Visible = false;
        mpeSubmitClaimSearchPop.Show();
        lblError.Visible = false;

    }

    protected void txtCheckForDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtICDPrCdDate.Text))
        {
            txtICD10ProcedureCodeDec1.Text = txtICD10ProcedureCodeDec.Value;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate5();</script>", false);

        }
        else
        {
            txtICD10ProcedureCodeDec1.Text = txtICD10ProcedureCodeDec.Value;
            ICDDateRequiredError1.Visible = false;
        }
    }

    protected void gvICD10ProcedureCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["ICDCodes"] != null)
        {
            //  gvICD10ProcedureCode.PageIndex = e.NewPageIndex;
            SetICDProcedureCodes((DataSet)ViewState["ICDCodes"]);
        }
        GetICDProcedureCodeData(null);

    }

    protected void txtICD10Procedurecode_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = LookupTableController.GetICDProcedureCode(txtICD10Procedurecode.Text, "", ddlICDPrCodeInstiutional.SelectedValue);
        //DataTable dt = GetMockData();

        if (Helper.HasRows(dt))
        {
            if (dt.Rows.Count > 0)
            {
                lblError.Visible = false;
                txtICD10ProcedureCodeDec.Value = dt.Rows[0]["LONG_DESC"].ToString();
                txtICD10ProcedureCodeDec1.Text = dt.Rows[0]["LONG_DESC"].ToString();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Invalid ICD code.";
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "Invalid ICD code.";
            txtICD10ProcedureCodeDec.Value = "";
            txtICD10ProcedureCodeDec1.Text = "";
        }


    }

    public void SaveToDbOnAdjust(DataTable dt)
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimIDInstitutional.Value, true));
        DataAccess.ExecuteStoredProcedure("usp_clear_IDC_Procedure_Code_Sequence_before_claim_adjust", parameters);

        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                var sequence = "";
                sequence = dt.Rows[i]["Sequence_No"].ToString();
                if (sequence == "Other")
                {
                    sequence = "2";
                }
                else if (sequence == "Principal")
                {
                    sequence= "3";
                }
                parms.Add("Claim_Id", hdnClaimIDInstitutional.Value);
                parms.Add("Sequence_NO", sequence);
                parms.Add("ICD_Version", dt.Rows[i]["ICD_Version"].ToString());
                parms.Add("Procedure_Code_Description", dt.Rows[i]["Procedure_Code_Description"].ToString());
                parms.Add("ICD_Procedure_Code", dt.Rows[i]["ICD_Procedure_Code"].ToString());
                parms.Add("ICD_Date", dt.Rows[i]["ICD_Date"].ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                svc.InsertPanelsData("claims_icd_procedurecode", parms);
            }
            SetICDProcedureCodes(null);
        }
    }
}