using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_TypeOfBill : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvSubmitClaimSearchPop.DataSource = null;
            gvSubmitClaimSearchPop.DataBind();
        }

    }


    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;


    protected void btnSearch_ClickTOB(object sender, EventArgs e)
    { 
         if ((string.IsNullOrEmpty(txtSearchTOBCode.Text)) && (string.IsNullOrEmpty(txtSearchTOBDesc.Text)))
    {

        lblError.Visible = true;
        lblError.Text = "Type of bill or Type of bill Description  is required for Search.";
            
        ModalPopupExtender mpeSubmitClaimSearchPop = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchPop");
        mpeSubmitClaimSearchPop.Show();
    }
    else
    {
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            this.gvSubmitClaimSearchPop.PageIndex = 0;
            lblError.Visible = false;
            GetFreshTOB();

            ModalPopupExtender mpeSubmitClaimSearchPop = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchPop");
            mpeSubmitClaimSearchPop.Show();

        }
    }


    public void GetFreshTOB()
    {
        DataTable dt = GetTOBData(gvSubmitClaimSearchPop.PageSize);
        if (Helper.HasRows(dt))
        {
            gvSubmitClaimSearchPop.DataSource = dt;
            gvSubmitClaimSearchPop.DataBind();
        }
        else
        {
            gvSubmitClaimSearchPop.DataSource = null;
            gvSubmitClaimSearchPop.DataBind();
        }
    }



    private DataTable GetTOBData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //DataSet ds;

        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records
        //DataTable dt = GetMockData();
        DataSet ds = GetRealData(txtSearchTOBCode.Text, txtSearchTOBDesc.Text);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    private DataSet GetRealData(string code = "", string desc = "")
    {

        DataSet dsTOB = MAXIMUS.Controllers.PDMS.LookupTableController.GetInstitutionalTypeOfBill(code, desc);       

        return dsTOB;
    }




    protected void lnkTOB_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;

        Session["TypeBillCode"] = cmdArgument;
        gvSubmitClaimSearchPop.DataSource = null;
        gvSubmitClaimSearchPop.DataBind();
        txtSearchTOBCode.Text = string.Empty;
        txtSearchTOBDesc.Text = string.Empty;
    }
    protected void gvSubmitClaimSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSubmitClaimSearchPop.PageIndex = e.NewPageIndex;
        GetFreshTOB();
        ModalPopupExtender mpeSubmitClaimSearchPop = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchPop");
        mpeSubmitClaimSearchPop.Show();
    }
}