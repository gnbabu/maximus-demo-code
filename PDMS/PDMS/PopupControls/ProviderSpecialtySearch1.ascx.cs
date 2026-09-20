using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Globalization;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Web.Security;
using Telerik.Web.UI.Skins;
using System.IO;
using System.Net.PeerToPeer.Collaboration;

public partial class PopupControls_ProviderSpecialtySearch1 : System.Web.UI.UserControl
{    
    DataSet dsResults = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        btnClear.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnClear, null) + ";");
        RefreshData();
    }
    protected void grdSpecSearchResults_PageIndexChanging(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        grdSpecSearchResults.CurrentPageIndex = e.NewPageIndex;
        ViewState["GridData"] = e.NewPageIndex;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNPI.Text) && string.IsNullOrEmpty(txtMedicaidID.Text))
        {
            lblErrorText.Visible = true;
            lblErrorText.Text = "* Please Enter NPI or Medicaid ID.";
            lnkExcel.Visible = false;
            grdSpecSearchResults.DataSource = null;
            grdSpecSearchResults.DataBind();
            return;
        }
        else if (!string.IsNullOrEmpty(txtNPI.Text) && txtNPI.Text.Length != 10)
        {
            lblErrorText.Visible = true;
            lblErrorText.Text = "* 10-digits NPI number is required.";
            grdSpecSearchResults.DataSource = null;
            grdSpecSearchResults.DataBind();
            lnkExcel.Visible = false;
            return;
        }
        else if (!string.IsNullOrEmpty(txtMedicaidID.Text) && txtMedicaidID.Text.Length != 7)
        {
            lblErrorText.Visible = true;
            lblErrorText.Text = "* 7-digits Medicaid ID is required.";
            grdSpecSearchResults.DataSource = null;
            grdSpecSearchResults.DataBind();
            lnkExcel.Visible = false;
            return;
        }

        RefreshData();
    }

    private void RefreshData()
    {
        if (!string.IsNullOrEmpty(txtNPI.Text) || !string.IsNullOrEmpty(txtMedicaidID.Text))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet ds = svc.GetProviderSpecialtySearchResults(txtNPI.Text, txtMedicaidID.Text);

            if (Helper.HasRows(ds))
            {
                string responseCode = ds.Tables[0].Rows[0]["RESPONSE_CODE"].ToString();

                if (responseCode == "300")
                {
                    lblErrorText.Visible = true;
                    lblErrorText.Text = "* NPI not registered in PNM. Please contact provider to verify NPI.";
                    grdSpecSearchResults.DataSource = null;
                    grdSpecSearchResults.DataBind();
                    lnkExcel.Visible = false;
                    return;
                }
                else if (responseCode == "301")
                {
                    lblErrorText.Visible = true;
                    lblErrorText.Text = "* Medicaid ID not registered in PNM. Please contact provider to verify.";
                    grdSpecSearchResults.DataSource = null;
                    grdSpecSearchResults.DataBind();
                    lnkExcel.Visible = false;
                    return;
                }
                else if (responseCode == "302")
                {
                    lblErrorText.Visible = true;
                    lblErrorText.Text = "* The NPI and Medicaid ID combination is not registered in PNM. Please contact provider to verify.";
                    grdSpecSearchResults.DataSource = null;
                    grdSpecSearchResults.DataBind();
                    lnkExcel.Visible = false;
                    return;
                }
                else if (responseCode == "200")
                {
                    dsResults = ds;
                    lblErrorText.Visible = false;
                    lblErrorText.Text = string.Empty;
                    lnkExcel.Visible = true;
                    grdSpecSearchResults.DataSource = ds;
                    grdSpecSearchResults.DataBind();
                }
            }
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        lblErrorText.Visible = false;
        lblErrorText.Text =string.Empty;
        txtNPI.Text = string.Empty;
        txtMedicaidID.Text = string.Empty;
        grdSpecSearchResults.DataSource = null;
        grdSpecSearchResults.DataBind();
        lnkExcel.Visible = false;
    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        grdSearchResultExport.DataSource = dsResults;
        grdSearchResultExport.DataBind();
        grdSearchResultExport.MasterTableView.ExportToExcel();
    }
}