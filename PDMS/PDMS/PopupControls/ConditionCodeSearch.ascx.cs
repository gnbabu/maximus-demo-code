using AjaxControlToolkit;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class PopupControls_ConditionCodeSearch : System.Web.UI.UserControl
{
    #region SVC
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

    #region Properties
    public string Condition_Code
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnCondition_Code.Value))
                return hdnCondition_Code.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnCondition_Code.Value = value.Trim();
            ClearHiddenField(value);
        }
    }

    public string Condition_Code_Description
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnCondition_Code_Description.Value))
                return hdnCondition_Code_Description.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnCondition_Code_Description.Value = value.Trim();
            ClearField(value);
        }
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        this.gvConditionCodeSearch.CurrentPageIndex = 0;
        if(!IsPostBack)
        {
            gvConditionCodeSearch.DataSource = null;
            gvConditionCodeSearch.DataBind();
        }
        //RefreshData();
    }
    protected void ClearHiddenField(string value)
    {
        hdnCondition_Code.Value = value;
    }
    protected void ClearField(string res)
    {
        hdnCondition_Code_Description.Value = res;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ModalPopupExtender modalConditionCode = (ModalPopupExtender)this.Parent.FindControl("mpeConditionCodeSearch");
        if (string.IsNullOrEmpty(txtConditonCode.Text) && string.IsNullOrEmpty(txtConditionCodeDesc.Text))
        {
            fieldRequireError.Text = "Condition code or description is required";
            modalConditionCode.Show();
            txtConditonCode.Text = "";
            txtConditionCodeDesc.Text = "";        
        }
        else
        {
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            this.gvConditionCodeSearch.CurrentPageIndex = 0;
            RefreshData();
            fieldRequireError.Text = "";
            modalConditionCode.Show();
            txtConditonCode.Text = "";
            txtConditionCodeDesc.Text = "";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void lnkConditionCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;

        txtConditonCode.Text = string.Empty;
        txtConditionCodeDesc.Text = string.Empty;
        DataSet dsConditionCodeDescription = svc.GetConditionCodeDescription(cmdArgument);
        hdnCondition_Code.Value = cmdArgument.ToString();
        hdnCondition_Code_Description.Value = dsConditionCodeDescription.Tables[0].Rows[0]["CLAIMS_CONDITION_CODE_DESCRIPTION"].ToString();
    }
    public void RefreshData()
    {
        DataTable dt = GetData(gvConditionCodeSearch.PageSize);
        if (Helper.HasRows(dt))
        {
            gvConditionCodeSearch.DataSource = dt;
            gvConditionCodeSearch.DataBind();
        }

    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
   
        string sortColWithDirection = this.gvConditionCodeSearch.GridViewSortDirection == SortDirection.Descending ? gvConditionCodeSearch.GridViewSortColumn + " DESC" : gvConditionCodeSearch.GridViewSortColumn;

        // If list of IDs has been passed in, display in search results list.
        DataSet dsCode = GetConditionCodeData();

        if (Helper.HasRows(dsCode))
        {
            return dsCode.Tables[0];
        }
        else return new DataTable();
    }

    private DataSet GetConditionCodeData()
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("CLAIMS_CONDITION_CODE", DbType.String, txtConditonCode.Text, true));
        parameters.Add(SqlParms.CreateParameter("CLAIMS_CONDITION_CODE_DESCRIPTION", DbType.String, txtConditionCodeDesc.Text, true));

        DataSet dataSetCondition = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Condition_Code", parameters, "CLAIMS_CONDITION_CODE");

        return dataSetCondition;
    }

    protected void gvConditionCodeSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //RefreshData();
    }

    protected void gvConditionCodeSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        //RefreshData();
    }
}