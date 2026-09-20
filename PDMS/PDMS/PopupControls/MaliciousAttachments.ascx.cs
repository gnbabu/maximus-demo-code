using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

public partial class PopupControls_MaliciousAttachments : System.Web.UI.UserControl
{
    private string p_ClaimTypeCode = string.Empty; //PA or Claim
    private string p_claimId = string.Empty; //ClaimId for claims or Medicaid Billing# for PA
    private string p_claimType = string.Empty;
    private string p_medicaidId = string.Empty;

    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(p_claimId))
                return p_claimId;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                p_claimId = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(p_claimType))
                return p_claimType;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                p_claimType = value.Trim();
            }
        }
    }

    public string ClaimTypeCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(p_ClaimTypeCode))
                return p_ClaimTypeCode;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                p_ClaimTypeCode = value.Trim();
                //if (!string.IsNullOrEmpty(p_claimId) && !string.IsNullOrEmpty(p_claimType))
                //    RefreshGrid(p_claimId, p_claimType, value);
            }
        }
    }

    public string MedicaidId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(p_medicaidId))
                return p_medicaidId;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                p_medicaidId = value.Trim();
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void RefreshGrid()
    {
        DataSet ds = ClaimsController.GetMaliciousDocs(p_claimId, p_claimType, p_ClaimTypeCode, p_medicaidId);
        //if (Helper.HasRows(ds))
        //{
        //}

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            gvMaliciousAttachments.DataSource = ds;
            gvMaliciousAttachments.DataBind();
        }
    }

    public void ClearGrid()
    {
        gvMaliciousAttachments.DataSource = null;
        gvMaliciousAttachments.DataBind();
    }
}