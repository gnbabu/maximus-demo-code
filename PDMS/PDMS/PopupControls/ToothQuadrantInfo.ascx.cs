using Amazon.Runtime.Internal.Transform;
using Corp.Core.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ToothQuadrantInfo : System.Web.UI.UserControl
{
    private bool fromInquirySvc = false;
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimId.Value))
                return hdnClaimId.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimId.Value = value.Trim();
        }
    }

    public bool FromInquirySvc
    {
        get
        {
            return fromInquirySvc;
        }
        set
        {
            fromInquirySvc = value;
        }
    }

    private DataSet dataSetToothQuadrantInformation = new DataSet();

    public DataSet dataSetToothQuadrant
    {
        get
        {
            if (!Helper.HasRows(dataSetToothQuadrantInformation))
            {
                if (Helper.HasRows(FetchToothInformation()))
                {
                    return dataSetToothQuadrantInformation;
                }
            }
            return dataSetToothQuadrantInformation;
        }
        set
        {
            if (value != null)
            {
                GetToothQuadrtantPanelInfo(value);
            }
        }
    }
    public string ICNNumber = string.Empty;
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
            //BindGrid();
        }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {
                sepToothQuadrantInfo.InnerText = sepToothQuadrantInfo.InnerText.Replace('-', '+');
                BindDropDowns();
            }
            catch (Exception ex)
            {

            }
        }
        BindGrid();
        //  btnAddToothQuadrantInfo.Attributes.Add("onclick", "TDisableEnableConditionAddButton();");
        SetButtonVisibility();
        
    }
    public void SetButtonVisibility()
    {
        //if (DisplayReadOnly == true)
            if (DisplayReadOnly == true)
            {
                divInsertTooth.Visible = false;
            }
            else
            {
                divInsertTooth.Visible = true;
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



    public void BindGrid()
    {
        if (!FromInquirySvc)
            GetToothQuadrtantPanelInfo(null);
    }
    private void BindDropDowns()
    {
        GetToothSurface();
        GetDetailNumber();

    }
    private void GetToothSurface()
    {
        ddlToothSurface1.Items.Clear();
        DataSet dataSetToothSurface = svc.GetToothSurface();
        if (Helper.HasRows(dataSetToothSurface))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            Helper.LoadList(ddlToothSurface1, dt, "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE", "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID", true);
        }


    }
    public void GetToothQuadrtantPanelInfo(DataSet dsToothQuadrant)
    {
        DataSet dsToothQuadrantInformation = new DataSet();
        if (!String.IsNullOrEmpty(hdnClaimId.Value) || !string.IsNullOrWhiteSpace(ICN))
        {

            if (!string.IsNullOrEmpty(hdnClaimId.Value))
            {
                if (Helper.HasRows(dsToothQuadrant))
                {
                    dsToothQuadrantInformation = dsToothQuadrant;
                }
                else
                {
                    dsToothQuadrantInformation = FetchToothInformation();
                }
            }
            if (Helper.HasRows(dsToothQuadrantInformation))
            {
                DataTable dt = new DataTable();
                dt = dsToothQuadrantInformation.Tables[0];
                string table = "";
                if (dt.Rows.Count > 0)
                {
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Tooth Number</th > <th style='width:10px; scope=' col'> Tooth Surface</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                    foreach (DataRow dr in dt.Rows)
                    {
                        string Tooth_hdnClaimId = hdnClaimId.Value;
                        string Claims_Tooth_and_Surface_Information_ID = dr["Claims_Tooth_and_Surface_Information_ID"].ToString();
                        string ToothServiceLine = dr["Service_Line"].ToString();
                        string ToothNumber = dr["Tooth_Number"].ToString();
                        string ToothSurface1 = dr["ToothSurface1"].ToString();
                        string ToothSurface2 = dr["ToothSurface2"].ToString();
                        string ToothSurface3 = dr["ToothSurface3"].ToString();
                        string ToothSurface4 = dr["ToothSurface4"].ToString();
                        string ToothSurface5 = dr["ToothSurface5"].ToString();
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + ToothServiceLine + "</span></td><td><span title='Line' class='tNumber'>" + ToothNumber + "</span></td><td><span  title='Line' class='tNumber'>" + ToothSurface1 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface2 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface3 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface4 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface5 + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditToothQuadrantInfoLineItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + Tooth_hdnClaimId + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteToothQuadrantInfoItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                        }
                        else {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + ToothServiceLine + "</span></td><td><span title='Line' class='tNumber'>" + ToothNumber + "</span></td><td><span  title='Line' class='tNumber'>" + ToothSurface1 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface2 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface3 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface4 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface5 + "</span></td></tr>";

                        }
                    }
                    table = table + "</tbody></table>";

                }
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnCancel.Enabled = true;
                ToothQuadrantInfoOutput.InnerHtml = table;
                //dataSetToothQuadrantInformation = dsToothQuadrantInformation;t
                //gvToothQuadrantInfo.DataSource = dsToothQuadrantInformation;
                //gvToothQuadrantInfo.DataBind();
                //if (gvToothQuadrantInfo.Rows.Count == 50)
                //{
                //    btnAddToothQuadrantInfo.Visible = false;
                //}
            }
            else
            {
                //gvToothQuadrantInfo.DataSource = null;
                //gvToothQuadrantInfo.DataBind();
                ToothQuadrantInfoOutput.InnerHtml = "";

            }

        }
        else
        {
            ToothQuadrantInfoOutput.InnerHtml = "";
        }

    }


    public DataSet FetchToothInformation()
    {
        DataSet dsToothQuadrant = new DataSet();
        if (!String.IsNullOrEmpty(hdnClaimId.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimId.Value);

            dataSetToothQuadrantInformation = dsToothQuadrant = svc.SelectPanelsData("Claims_Tooth_and_Surface_Information", parms);
        }
        return dsToothQuadrant;
    }

    private void EditGetToothSurface(DropDownList dropdown)
    {
        ddlToothSurface1.Items.Clear();
        DataSet dataSetToothSurface = svc.GetToothSurface();
        if (Helper.HasRows(dataSetToothSurface))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            Helper.LoadDropDown(dropdown, dt, "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE", "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID", true, true);
        }
    }

    public DataTable EditGetToothSurface1(DropDownList dropdown)
    {
        ddlToothSurface1.Items.Clear();
        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(dropdown.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];

            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(dropdown.SelectedValue))
                               .CopyToDataTable();
        }

        return selectedTable;
    }

    public DataTable EditGetToothSurface2(DropDownList dropdown, DropDownList ddlToothSurface1)
    {
        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(dropdown.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface1.SelectedValue) &&
                              r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(dropdown.SelectedValue))
                               .CopyToDataTable();
        }

        return selectedTable;
    }

    public DataTable EditGetToothSurface3(DropDownList dropdown, DropDownList ddlToothSurface1, DropDownList ddlToothSurface2)
    {
        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(dropdown.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface1.SelectedValue) &&
                              r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface2.SelectedValue) &&
                              r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(dropdown.SelectedValue))
                               .CopyToDataTable();
        }

        return selectedTable;
    }

    public DataTable EditGetToothSurface4(DropDownList dropdown, DropDownList ddlToothSurface1, DropDownList ddlToothSurface2, DropDownList ddlToothSurface3)
    {
        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(dropdown.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface1.SelectedValue) &&
                              r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface2.SelectedValue) &&
                              r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface3.SelectedValue) &&
                              r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(dropdown.SelectedValue))
                               .CopyToDataTable();
        }

        return selectedTable;
    }

    protected void txtToothNumber_TextChanged(object sender, EventArgs e)
    {
        bool status = validateToothNumber(txtToothNumber.Text);
        if (status == false)
        {
            lblToothNumberInvalid.Text = "*Tooth number is invalid";
            lblToothNumberInvalid.Visible = true;
        }
        else
        {
            lblToothNumberInvalid.Text = "";
            lblToothNumberInvalid.Visible = false;
        }
    }

    protected void ddlToothSurface1_Selected_IndexChanged(object sender, EventArgs e)
    {
        Helper.LoadList(ddlToothSurface2, GetToothSurface1(), "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE", "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID", true);
    }
    protected void ddlToothSurface2_Selected_IndexChanged(object sender, EventArgs e)
    {
        Helper.LoadList(ddlToothSurface3, GetToothSurface2(), "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE", "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID", true);
    }
    protected void ddlToothSurface3_Selected_IndexChanged(object sender, EventArgs e)
    {
        Helper.LoadList(ddlToothSurface4, GetToothSurface3(), "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE", "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID", true);
    }
    protected void ddlToothSurface4_Selected_IndexChanged(object sender, EventArgs e)
    {
        Helper.LoadList(ddlToothSurface5, GetToothSurface4(), "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE", "PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID", true);
    }
    public DataTable GetToothSurface1()
    {

        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ddlToothSurface1.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface1.SelectedValue))
                               .CopyToDataTable();
        }
        return selectedTable;

    }
    public DataTable GetToothSurface2()
    {
        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ddlToothSurface2.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface1.SelectedValue) &&
                              r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface2.SelectedValue))
                               .CopyToDataTable();
        }

        return selectedTable;

    }
    public DataTable GetToothSurface3()
    {

        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ddlToothSurface3.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface1.SelectedValue) &&
                                           r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface2.SelectedValue) &&
                                           r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface3.SelectedValue))
                               .CopyToDataTable();
        }

        return selectedTable;

    }
    public DataTable GetToothSurface4()
    {

        DataSet dataSetToothSurface = svc.GetToothSurface();
        DataTable selectedTable = new DataTable();
        if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ddlToothSurface4.SelectedItem.Text))
        {
            DataTable dt = dataSetToothSurface.Tables[0];
            selectedTable = dt.AsEnumerable()
                               .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface1.SelectedValue) &&
                                           r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface2.SelectedValue) &&
                                           r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface3.SelectedValue) &&
                                           r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ddlToothSurface4.SelectedValue))
                               .CopyToDataTable();
        }
        return selectedTable;

    }
    protected void btnToothQuadrantInfoAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (validateToothNumber(txtToothNumber.Text) && (!string.IsNullOrEmpty(hdnClaimId.Value)))
            {
                int toothPanelId = 0;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms = new Dictionary<string, string>();
                parms.Add("Service_Line", ddlToothServiceLine.SelectedValue.ToString());
                parms.Add("Tooth_Number", txtToothNumber.Text.ToString());
                parms.Add("Tooth_Surface1", ddlToothSurface1.SelectedValue.ToString());
                parms.Add("Tooth_Surface2", ddlToothSurface2.SelectedValue.ToString());
                parms.Add("Tooth_Surface3", ddlToothSurface3.SelectedValue.ToString());
                parms.Add("Tooth_Surface4", ddlToothSurface4.SelectedValue.ToString());
                parms.Add("Tooth_Surface5", ddlToothSurface5.SelectedValue.ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Claim_id", hdnClaimId.Value);
                toothPanelId = svc.InsertPanelsData("Claims_Tooth_and_Surface_Information", parms);
                hdnToothPanelId.Value = toothPanelId.ToString();
                GetDetailNumber();
                ClearAllToothQuadrantFields();

                GetToothQuadrtantPanelInfo(null);
                toothValidator.Visible = false;
            }
            else
            {
                toothValidator.Visible = true;
            }
        }


    }
    public void ClearAllToothQuadrantFields()
    {
        ddlToothServiceLine.ClearSelection();
        txtToothNumber.Text = "";
        ddlToothSurface1.ClearSelection();
        ddlToothSurface2.ClearSelection();
        ddlToothSurface3.ClearSelection();
        ddlToothSurface4.ClearSelection();
        ddlToothSurface5.ClearSelection();
        //ToothQuadrantInfoOutput.InnerHtml = "";

    }
    public void GetDetailNumber()
    {
        ddlToothServiceLine.Items.Clear();
        DataSet dstoothServiceLine = new DataSet();
        if (!string.IsNullOrEmpty(ClaimId))
        {
            dstoothServiceLine = svc.GetToothServiceLine(Convert.ToInt32(hdnClaimId.Value));
            if (Helper.HasRows(dstoothServiceLine))
            {
                Helper.LoadList(ddlToothServiceLine, dstoothServiceLine.Tables[0], "Service_Line", "Service_Line", true);

            }
        }


    }

    public Boolean validateToothNumber(string txtValue)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlHelper.CreateParameter("TOOTH_NUMBER", DbType.String, txtValue, true));
        DataSet dsToothNumber = MAXIMUS.Core.Libraries.DataAccess.ExecuteStoredProcedure("usp_SelectPrior_auth_submit_claim_tooth_number", param, "TOOTH_NUMBER");
        if (Helper.HasRows(dsToothNumber))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!(ddlToothServiceLine.SelectedIndex > 0))
        {
            GetDetailNumber();
            ClearAllToothQuadrantFields();
        }
    }
    public void SaveToDbonAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int toothPanelId = 0;
                int serviceline = Convert.ToInt32(dt.Rows[i]["Service_Line"].ToString());
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms = new Dictionary<string, string>();
                parms.Add("Service_Line", serviceline.ToString());
                parms.Add("Tooth_Number", dt.Rows[i]["Tooth_Number"].ToString());
                parms.Add("Tooth_Surface1", GetToothSurfaceId(dt.Rows[i]["Toothsurface1"].ToString()));
                parms.Add("Tooth_Surface2", GetToothSurfaceId(dt.Rows[i]["Toothsurface2"].ToString()));
                parms.Add("Tooth_Surface3", GetToothSurfaceId(dt.Rows[i]["Toothsurface3"].ToString()));
                parms.Add("Tooth_Surface4", GetToothSurfaceId(dt.Rows[i]["Toothsurface4"].ToString()));
                parms.Add("Tooth_Surface5", GetToothSurfaceId(dt.Rows[i]["Toothsurface5"].ToString()));
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Claim_id", hdnClaimId.Value);
                toothPanelId = svc.InsertPanelsData("Claims_Tooth_and_Surface_Information", parms);
                hdnToothPanelId.Value = toothPanelId.ToString();
            }
            GetToothQuadrtantPanelInfo(null);
        }
    }
    private string GetToothSurfaceId(string toothSurface)
    {
        string name = string.Empty;
        if (!string.IsNullOrWhiteSpace(toothSurface) && Helper.HasRows(this.WorkflowPage.OtherPayerSequenceTable.Tables[0]))
        {
            var dt1 = this.WorkflowPage.ToothSurfaceNumber.Tables[0].AsEnumerable().Where(r => r.Field<String>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE") == toothSurface);
            var newDt = dt1.CopyToDataTable();
            if (Helper.HasRows(newDt))
            {
                name = newDt.Rows[0]["PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID"].ToString();
            }
        }
        return name;
    }
}
