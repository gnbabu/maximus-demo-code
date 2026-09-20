using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using MAXIMUS.Core.Libraries;
using NPOI.HPSF;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_ProviderAdminAgentAccounts : System.Web.UI.Page
{
    private static DataSet dsProvInfo;
    public bool isDeemedEligible;
    private static int PageSize = 5;
    DataTable dtDeactiveUserID;
    private bool isCostRptMgmtAgent = false; 
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CurrentPage = 1;
            ViewState["NextPage"] = PageSize + CurrentPage;
            ViewState["PreviousPage"] = 1;

            HttpContext.Current.Session["GridAgentChecked"] = false;
        }

        isCostRptMgmtAgent = (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent)
               && Helper.IsUserInSubRoles(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.CostReportManagementAgent));

        if(isCostRptMgmtAgent)
        {
            btnChangeOwner.Visible = false;
            divChangeAdmin.Visible = false;
        }

        LoadMedicaidDropDown();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        isDeemedEligible = psc.IsDeemedEligble(HttpContext.Current.User.Identity.Name);

        LoadGrid();
        btnSearchAgent.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSearchAgent.ValidationGroup +
           "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSearchAgent, null) + ";");
        btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
           "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnAddUser.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnAddUser.ValidationGroup +
          "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnAddUser, null) + ";");
        //}
    }
    protected void ddlMedicaidID_SelectedIndexChanged(object sender, EventArgs e)
    {
        Page.Validate();
        CurrentPage = 1;
        ViewState["NextPage"] = PageSize + CurrentPage;
        ViewState["PreviousPage"] = 1;
        if (ddlMedicaidID.SelectedIndex == 0)
        {
            grdAgentRoles.DataSource = new DataTable();
            grdAgentRoles.DataBind();
            grdAgentRoles.Visible = false;
            grdAgentRoles.Columns.Clear();
            grdDeactivateUser.Visible = false;
            grdDeactivateUser.DataSource = new DataTable();
            grdDeactivateUser.DataBind();
            grdDeactivateUser.Columns.Clear();
            rptPaging.Visible = false;
            lbFirst.Visible = false;
            lbPrevious.Visible = false;
            lbNext.Visible = false;
            lbLast.Visible = false;
            lblpage.Visible = false;
            return;
        }
        grdAgentRoles.Visible = true;
        grdDeactivateUser.Visible = true;
        URP03_ERR.Text = string.Empty;
        URP03_ERR.Visible = false;
        rptPaging.Visible = true;
        lbFirst.Visible = true;
        lbPrevious.Visible = true;
        lbNext.Visible = true;
        lbLast.Visible = true;
        lblpage.Visible = true;
        hdnRegID.Value = ddlMedicaidID.SelectedValue;

        DataView dsProvView = dsProvInfo.Tables[0].DefaultView;
        dsProvView.RowFilter = "RegID = " + Convert.ToInt32(hdnRegID.Value);
        DataTable dtProvinfo = dsProvView.ToTable();
        DataRow dr = dtProvinfo.Rows[0];
        HttpContext.Current.Session["GridAgentChecked"] = false;
        lblName.Text = Helper.GetString("ProviderName", dr);
        hdnProvAdminUserID.Value = isCostRptMgmtAgent ? Helper.GetString("PROVIDER_ADMIN_USER_ID", dr) : Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Session["ProviderType"] = psc.GetProviderTypeIdByRegId(Convert.ToInt32(hdnRegID.Value)).Trim();
        
    }

    private void LoadMedicaidDropDown()
    {
        int defaultPageSize = 15;
        int defaultStartRowIndex = 0;
        bool getTotalRowCount = true;
        string defaultSortColumn = "ProviderName";
        int totalResultCount = 0;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (isCostRptMgmtAgent)
        {
            dsProvInfo = psc.SelectRegistrationsByAgentUserIDAndSubRole(Helper.GetUserId(HttpContext.Current.User.Identity.Name), CON.CostReportManagementAgent);
            if (ddlMedicaidID.Items.Count == 0)
                Helper.LoadList(this.ddlMedicaidID, dsProvInfo.Tables[0], "MedicaidID", "RegID", true);
        }
        else
        {
            dsProvInfo = psc.SelectAllRegistrationsByUserID(Helper.GetUserId(HttpContext.Current.User.Identity.Name), 0, CON.ProviderCategoryTypeID.GroupMemberProfile, defaultSortColumn, defaultPageSize, defaultStartRowIndex, getTotalRowCount, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), out totalResultCount);
            if (ddlMedicaidID.Items.Count == 0)
                Helper.LoadList(this.ddlMedicaidID, dsProvInfo.Tables[1], "MedicaidID", "RegID", true);
        }
        
    }

    protected void LoadGrid()
    {
        if (string.IsNullOrEmpty(hdnRegID.Value))
        {
            lbFirst.Visible = false;
            lbPrevious.Visible = false;
            lbNext.Visible = false;
            lbLast.Visible = false;
            lblpage.Visible = false;
            return;
        }
        else
        {
			lbFirst.Visible = true;
			lbPrevious.Visible = true;
			lbNext.Visible = true;
			lbLast.Visible = true;
			lblpage.Visible = true;
		}
        BindAgentData((CurrentPage - 1) * 5);//Offset set to 0
        ColorPagerFirstPage();
    }

    private void GetAgentCount()
    {
        if (string.IsNullOrEmpty(hdnRegID.Value))
            return;
		string agentUserName = null;
		agentUserName = String.IsNullOrEmpty(txtSeltAgent.Text.Trim()) ? null : txtSeltAgent.Text.Trim();
		PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet AgentsCountDS = psc.GetAgentRolesByProviderAdminCount(hdnProvAdminUserID.Value, Convert.ToInt32(hdnRegID.Value), agentUserName);
        Session["AgentsCount"] = AgentsCountDS.Tables[0].Rows[0].ItemArray[0];
      
        CurrentPage = 1;
		BindDataIntoRepeater(CurrentPage);
    }

    private void BindAgentData(int Offset)
    {
		if (Offset < 0)
            Offset = 0;
        string agentUserName = null;
        agentUserName = String.IsNullOrEmpty(txtSeltAgent.Text.Trim()) ? null : txtSeltAgent.Text.Trim();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet AgentsDS = psc.SelectAgentRolesByProviderAdmin(hdnProvAdminUserID.Value, Convert.ToInt32(hdnRegID.Value), Offset, PageSize, agentUserName, isCostRptMgmtAgent);
        
        if (!Helper.HasRows(AgentsDS))
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "No Agents are mapped to this Mediciaid ID.<br />";
            grdDeactivateUser.DataSource = null;
            grdDeactivateUser.DataBind();

            grdAgentRoles.DataSource = null;
            grdAgentRoles.DataBind();
            lbFirst.Visible = false;
            lbPrevious.Visible = false;
            lbNext.Visible = false;
            lbLast.Visible = false;
            lblpage.Visible = false;
            return;
        }
        else
        {
            URP03_ERR.Visible = false;
        }
        grdAgentRoles.DataSource = new DataTable();
        grdAgentRoles.DataBind();
        grdAgentRoles.Columns.Clear();
        grdDeactivateUser.Columns.Clear();
        BoundField userNameColumn = new BoundField();
        userNameColumn.HeaderText = "Agent Role";
        userNameColumn.DataField = "AGENT_SUB_ROLES_DESC";
        userNameColumn.HeaderStyle.Width = 300;
        grdAgentRoles.Columns.Add(userNameColumn);

        AddTemplateFieldToGridView(grdAgentRoles, "AGENT_SUB_ROLES_ID");

        var ownerUserName = HttpContext.Current.User.Identity.Name;
        DataTable dtAgentRoles = AgentsDS.Tables[1];

        // Leave out the columns with AGENT_SUB_ROLES_DESC,AGENT_SUB_ROLES_ID and the rest of them should have the checkboxes
        for (int columnNum = 2; columnNum < dtAgentRoles.Columns.Count; columnNum++)
        {
            var columnName = dtAgentRoles.Columns[columnNum].ColumnName;
            /*if (columnName == ownerUserName)
            {
                columnName += "*";
                dtAgentRoles.Columns[columnNum].ColumnName += "*";
            }*/
            TemplateField bfield = new TemplateField();
            bfield.HeaderText = columnName;
            //Initalize the DataField value.
            bfield.HeaderTemplate = new GridViewCheckBoxTemplate(ListItemType.Header, columnName);
            //Initialize the HeaderText field value.
            bfield.ItemTemplate = new GridViewCheckBoxTemplate(ListItemType.Item, columnName);

            grdAgentRoles.Columns.Add(bfield);
        }

        DataTable dtDeactiveUser = AgentsDS.Tables[0];
        dtDeactiveUserID = AgentsDS.Tables[0];
        BoundField pageNameForDeactivateUser = new BoundField();
        pageNameForDeactivateUser.HeaderText = "Action";
        pageNameForDeactivateUser.DataField = "Action";
        pageNameForDeactivateUser.HeaderStyle.Width = 300;
        grdDeactivateUser.Columns.Add(pageNameForDeactivateUser);

        for (int columnNum = 1; columnNum < dtDeactiveUser.Columns.Count; columnNum++)
        {
            var columnName = dtDeactiveUser.Columns[columnNum].ColumnName;
            /*if (columnName == ownerUserName)
            {
                columnName += "*";
                dtDeactiveUser.Columns[columnNum].ColumnName += "*";

                if (dtDeactiveUser.Rows.Count > 0)
                    dtDeactiveUser.Rows[0][columnNum] = System.DBNull.Value;

                BoundField ownerUserColumn = new BoundField();
                ownerUserColumn.HeaderText = columnName;
                ownerUserColumn.DataField = columnName;

                grdDeactivateUser.Columns.Add(ownerUserColumn);

            }
            else
            {
                
            }*/
          
            TemplateField bfield = new TemplateField();
            bfield.HeaderText = columnName;
            //Initalize the DataField value.
            bfield.HeaderTemplate = new GridViewButtonTemplate(ListItemType.Header, columnName, new EventHandler(grdDeactivateUser_ButtonClick));
            //Initialize the HeaderText field value.
            bfield.ItemTemplate = new GridViewButtonTemplate(ListItemType.Item, columnName, new EventHandler(grdDeactivateUser_ButtonClick));
            grdDeactivateUser.Columns.Add(bfield);
        }

        grdDeactivateUser.DataSource = dtDeactiveUser;
        /*grdDeactivateUser.DataSource = dtDeactiveUser.AsEnumerable()
                                .Where(x => x.Field<string>("Action") != "UserID")
                                .CopyToDataTable();*/
        grdDeactivateUser.DataBind();

        grdAgentRoles.DataSource = dtAgentRoles;
        grdAgentRoles.DataBind();
    }

    private void grdDeactivateUser_ButtonClick(object sender, EventArgs e)
    {
        Button deactivateUser = (Button)sender;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.DeleteProviderAgentMapping(Convert.ToInt32(hdnRegID.Value), hdnProvAdminUserID.Value, Helper.GetUserId(deactivateUser.CommandArgument));
        LoadGrid();
    }

    private void AddTemplateFieldToGridView(GridView gridView, string columnName)
    {
        TemplateField templateField = new TemplateField();
        templateField.HeaderText = columnName;
        //Initalize the DataField value.
        templateField.HeaderTemplate = new GridViewLabelTemplate(ListItemType.Header, columnName);
        //Initialize the HeaderText field value.
        templateField.ItemTemplate = new GridViewLabelTemplate(ListItemType.Item, columnName);
        templateField.Visible = false;
        gridView.Columns.Add(templateField);
    }
    protected void grdDeactivateUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "Agent Name")
            {
                DataRow row = dtDeactiveUserID.Rows[0];
                foreach (DataControlField column in grdDeactivateUser.Columns)
                {
                    int columnIndex = grdDeactivateUser.Columns.IndexOf(column);
                    if (columnIndex == 0)
                        continue;
                    Button button1 = (Button)e.Row.Cells[1].FindControl("permCheckBox_" + column.HeaderText);
                    button1.Visible = false;
                    e.Row.Cells[columnIndex].Text = row[column.HeaderText].ToString();
                }
            }
            foreach (TableCell cell in e.Row.Cells)
            { cell.Font.Bold = true; }
            e.Row.Font.Bold = true;
        }

    }
    protected void grdAgentRoles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "UserID")
                e.Row.Visible = false;

            if (e.Row.Cells[0].Text == "Deactivate User")
            { // Add deactivate user stuff here.

            }

            //SAM573
            if (isCostRptMgmtAgent)
            {
                if (e.Row.Cells[0].Text == "Hosp Cost Report Upload" || e.Row.Cells[0].Text == "Prepare Save LTC Cost Report" || e.Row.Cells[0].Text == "View LTC Cost Report" ||
                    e.Row.Cells[0].Text == "Prepare Save MSP Cost Reports" || e.Row.Cells[0].Text == "View MSP Cost Reports" || e.Row.Cells[0].Text == "View MSP Cost Report Due Date" ||
                    e.Row.Cells[0].Text == "FQHC Cost Report Upload" || e.Row.Cells[0].Text == "View FQHC Cost Report" || e.Row.Cells[0].Text == "RHC Cost Report Upload" ||
                    e.Row.Cells[0].Text == "View RHC Cost Report" || e.Row.Cells[0].Text == "View Hospital Cost Report" || e.Row.Cells[0].Text == "OHF Cost Report Upload" ||
                    e.Row.Cells[0].Text == "View OHF Cost Report")
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }

            if (e.Row.Cells[0].Text == "Deemed Eligibility")
            {
                e.Row.Enabled = isDeemedEligible;
            }

            if (e.Row.Cells[0].Text == "Hosp Cost Report Upload" || e.Row.Cells[0].Text == "View Hospital Cost Report")
            {
                if (Session["ProviderType"] != null)
                {
                    if (Session["ProviderType"].ToString() == CON.MMISProviderType.Hospital || Session["ProviderType"].ToString() == CON.MMISProviderType.Psychiatric_Hospital)
                    {
                        e.Row.Enabled = true;
                    }
                    else
                    {
                        e.Row.Enabled = false;
                    }
                }
            }
            else if (e.Row.Cells[0].Text == "Sign Approve LTC Cost Report" || e.Row.Cells[0].Text == "MDS Report" ||
                    e.Row.Cells[0].Text == "Prepare Save LTC Cost Report" || e.Row.Cells[0].Text == "View LTC Cost Report")
            {
                if (Session["ProviderType"].ToString() == CON.MMISProviderType.NURSING_FACILITY || Session["ProviderType"].ToString() == CON.MMISProviderType.NON_STATE_OPERATED_ICF_MR || Session["ProviderType"].ToString() == CON.MMISProviderType.STATE_OPERATED_ICF_MR)
                { 
                    if(isCostRptMgmtAgent)
                    {
                        if (e.Row.Cells[0].Text == "Prepare Save LTC Cost Report" || e.Row.Cells[0].Text == "View LTC Cost Report")
                        {
                            e.Row.Enabled = true;
                        }
                        else
                        { 
                            e.Row.Enabled = false;
                        }
                    }                     
                    else
                      e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }            
            else if (e.Row.Cells[0].Text == "Prepare Save MSP Cost Reports" || e.Row.Cells[0].Text == "Sign Certify MSP Cost Reports" ||
                    e.Row.Cells[0].Text == "View MSP Cost Reports" || e.Row.Cells[0].Text == "View MSP Cost Report Due Date")
            {
                if (Session["ProviderType"].ToString() == CON.MMISProviderType.MEDICAID_SCHOOL_PROGRAM)
                {
                    if (isCostRptMgmtAgent)
                    {
                        if (e.Row.Cells[0].Text == "Prepare Save MSP Cost Reports" || e.Row.Cells[0].Text == "View MSP Cost Reports" || e.Row.Cells[0].Text == "View MSP Cost Report Due Date")
                        {
                            e.Row.Enabled = true;
                        }
                        else
                        {
                            e.Row.Enabled = false;
                        }
                    }
                    else
                        e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }
            else if (e.Row.Cells[0].Text == "FQHC Cost Report Upload" || e.Row.Cells[0].Text == "View FQHC Cost Report")
            {
                if (Session["ProviderType"].ToString() == CON.MMISProviderType.FEDERALLY_QUALIFIED_HEALTH_CENTER)
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }
            else if (e.Row.Cells[0].Text == "RHC Cost Report Upload" || e.Row.Cells[0].Text == "View RHC Cost Report" ||
                    e.Row.Cells[0].Text == "Lead Investigation Cost Report Upload" || e.Row.Cells[0].Text == "View LI Cost Report")
            {
                if (Session["ProviderType"].ToString() == CON.MMISProviderType.Rural_Health_Clinic)
                {
                    if (isCostRptMgmtAgent)
                    {
                        if (e.Row.Cells[0].Text == "RHC Cost Report Upload" || e.Row.Cells[0].Text == "View RHC Cost Report")
                        {
                            e.Row.Enabled = true;
                        }
                        else
                        {
                            e.Row.Enabled = false;
                        }
                    }
                    else
                        e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }
            else if (e.Row.Cells[0].Text == "OHF Cost Report Upload" || e.Row.Cells[0].Text == "View OHF Cost Report")
            {
                if (Session["ProviderType"].ToString() == CON.MMISProviderType.OUTPATIENT_HEALTH_FACILITY)
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }
            else if (e.Row.Cells[0].Text == "Sign Certify Hospital Cost Report")
            {
                if (!isCostRptMgmtAgent && (Session["ProviderType"].ToString() == CON.MMISProviderType.Hospital || Session["ProviderType"].ToString() == CON.MMISProviderType.Psychiatric_Hospital))
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }

            else if (e.Row.Cells[0].Text == "Sign Certify FQHC Cost Report ")
            {
                if (!isCostRptMgmtAgent && (Session["ProviderType"].ToString() == CON.MMISProviderType.FEDERALLY_QUALIFIED_HEALTH_CENTER))
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }

            else if (e.Row.Cells[0].Text == "Sign Certify RHC Cost Report")
            {
                if (!isCostRptMgmtAgent && (Session["ProviderType"].ToString() == CON.MMISProviderType.Rural_Health_Clinic))
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }

            else if (e.Row.Cells[0].Text == "Sign Certify OHF Cost Report")
            {
                if (!isCostRptMgmtAgent && (Session["ProviderType"].ToString() == CON.MMISProviderType.OUTPATIENT_HEALTH_FACILITY))
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }
            else if(e.Row.Cells[0].Text == "Cost Report Management Agent" && !isCostRptMgmtAgent)
            {
                if(Session["ProviderType"].ToString() == CON.MMISProviderType.Hospital || Session["ProviderType"].ToString() == CON.MMISProviderType.OUTPATIENT_HEALTH_FACILITY ||
                    Session["ProviderType"].ToString() == CON.MMISProviderType.Rural_Health_Clinic || Session["ProviderType"].ToString() == CON.MMISProviderType.FEDERALLY_QUALIFIED_HEALTH_CENTER ||
                        Session["ProviderType"].ToString() == CON.MMISProviderType.NURSING_FACILITY || Session["ProviderType"].ToString() == CON.MMISProviderType.STATE_OPERATED_ICF_MR ||
                        Session["ProviderType"].ToString() == CON.MMISProviderType.NON_STATE_OPERATED_ICF_MR || Session["ProviderType"].ToString() == CON.MMISProviderType.MEDICAID_SCHOOL_PROGRAM)
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }

            if (((DataRowView)e.Row.DataItem).Row["AGENT_SUB_ROLES_ID"].GetType() == typeof(System.DBNull))
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Font.Bold = true;
                }
                e.Row.Font.Bold = true;
            }

            //SAM743
            if (e.Row.Cells[0].Text == "MCO Agent") 
            {
                if (ddlMedicaidID.SelectedItem.Text == "0077148" || ddlMedicaidID.SelectedItem.Text == "0077193" || ddlMedicaidID.SelectedItem.Text == "0077186" ||
                 ddlMedicaidID.SelectedItem.Text == "0077115" || ddlMedicaidID.SelectedItem.Text == "0462293" || ddlMedicaidID.SelectedItem.Text == "0462285" ||
                 ddlMedicaidID.SelectedItem.Text == "0464229")
                {
                    e.Row.Enabled = true;
                }
                else
                {
                    e.Row.Enabled = false;
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (ddlMedicaidID.SelectedIndex == 0 || ddlMedicaidID.SelectedIndex == -1)
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "Select a Medicaid ID.<br />";
            return;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        
        DataTable dt = new DataTable("AGENT_ROLE_STAGING");
        DataRow dr;
        dt.Columns.Add("REG_ID", typeof(int));
        dt.Columns.Add("PROVIDER_ADMIN_USER_ID", typeof(string));       
        dt.Columns.Add("AGENT_USER_ID", typeof(string));
        dt.Columns.Add("AGENT_SUB_ROLES_ID", typeof(string));
        dt.Columns.Add("IS_SELECTED", typeof(int));

        foreach (DataControlField column in grdAgentRoles.Columns)
        {
            string userName = string.Empty;
            int columnIndex = grdAgentRoles.Columns.IndexOf(column);

            if (columnIndex == 0 || columnIndex == 1)
                continue;

            var columnName = column.HeaderText.Replace("*", "");
            var userId = Membership.GetUser(columnName);
           
            foreach (GridViewRow row in grdAgentRoles.Rows)
            {

                CheckBox checkBox = (CheckBox)row.Cells[columnIndex].FindControl("permCheckBox_" + column.HeaderText);
                Label label = (Label)row.Cells[1].FindControl("permuserLabel" + "AGENT_SUB_ROLES_ID");

                if (columnName == "AGENT_SUB_ROLES_ID" || label.Text == "0")
                    continue; // this is the hidden row for the user             

              
                if (checkBox.Checked)
                {
                    dr = dt.NewRow();
                    dr["REG_ID"] = Convert.ToInt32(hdnRegID.Value);
                    dr["PROVIDER_ADMIN_USER_ID"] = hdnProvAdminUserID.Value;
                    dr["AGENT_USER_ID"] = userId.ProviderUserKey.ToString();
                    dr["AGENT_SUB_ROLES_ID"] = label.Text;
                    dr["IS_SELECTED"] = 1;
                    dt.Rows.Add(dr);
                }
            }            
        }
        Dictionary<string, object> parms = new Dictionary<string, object>();
        parms.Add("AGENT_ROLE_TABLE", dt);
        psc.SaveAgentRolesByProviderAdmin(parms);
        
        HttpContext.Current.Session["GridAgentChecked"] = false;
    }

    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        if (ddlMedicaidID.SelectedIndex == 0 || ddlMedicaidID.SelectedIndex == -1)
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "Select a Medicaid ID.<br />";
            return;
        }
        mpeAddAgent.Show();

        HttpContext.Current.Session["GridAgentChecked"] = false;
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session["GridAgentChecked"] = false;
        Response.Redirect("~/Process/ProviderHomeNew.aspx");
    }

    protected void btnChangeOwner_Click(object sender, EventArgs e)
    {
        MembershipUser user = Membership.GetUser(txtChgAdmin.Text);
        if (user == null)
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "User ID entered does not exist.<br />";
            return;
        }

        if (Helper.IsMITSuser(Helper.GetUserId(txtChgAdmin.Text).ToString()))
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "User ID entered should be a OH|ID account.<br />";
            return;
        }
        if (!Roles.IsUserInRole(txtChgAdmin.Text, CON.UserRoleType.ProviderAdministrator))
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "User ID entered cannot be assigned as the Provider Administrator.<br />";
            return;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.TransferRegistrationToNewUser(Convert.ToInt32(hdnRegID.Value), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Helper.GetUserId(txtChgAdmin.Text).ToString(), true);
        // Response.Redirect("~/Process/ProviderHomeNew.aspx");
        ListItem removeItem = ddlMedicaidID.Items.FindByValue(hdnRegID.Value);
        ddlMedicaidID.Items.Remove(removeItem);

        ddlMedicaidID.SelectedIndex = 0;

        grdAgentRoles.DataSource = new DataTable();
        grdAgentRoles.DataBind();
        grdAgentRoles.Visible = false;
        grdAgentRoles.Columns.Clear();
        grdDeactivateUser.Visible = false;
        grdDeactivateUser.DataSource = new DataTable();
        grdDeactivateUser.DataBind();
        grdDeactivateUser.Columns.Clear();
        rptPaging.Visible = false;
        lbFirst.Visible = false;
        lbPrevious.Visible = false;
        lbNext.Visible = false;
        lbLast.Visible = false;
        lblpage.Visible = false;

        LoadGrid();
    }

    protected void Validate_UserNameExists(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Membership.GetUser(UserName.Text) != null && Roles.IsUserInRole(UserName.Text, "ProviderAgent");
    }
    protected bool ValidateEmail()
    {
        string email = string.Empty;
        bool isValid = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetUsermembershipInfoByUserName(UserName.Text);
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                email = row["email"].ToString();
            }

            if (email.ToLower() != Email.Text.ToLower())
            {
                lblmpeError.Visible = true;
                lblmpeError.Text = "*User ID not Found";
                //CustomValidator val = new CustomValidator();
                //val.IsValid = false;
                //val.ErrorMessage = "*User ID not Found";
                //val.ValidationGroup = "AddAgentValidationSummary";
                //this.Page.Validators.Add(val);
                isValid = false;
            }
        }
        return isValid;
    }
    protected void btnSaveAddAgent_Click(object sender, EventArgs e)
    {
        if (ddlMedicaidID.SelectedIndex == 0 || ddlMedicaidID.SelectedIndex == -1)
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "Select a Medicaid ID.<br />";
            return;
        }
        bool isValid = true;
        isValid = ValidateEmail();
        Page.Validate("AddAgentValidationSummary");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v.ValidationGroup.Equals("AddAgentValidationSummary") && !v.IsValid)
                {
                    isValid = false;
                }
            }
            catch
            {
                continue;
            }
        }

        if (!isValid || !Page.IsValid)
        {
            mpeAddAgent.Show();
            return;
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertNewProviderAgent(Helper.GetUserId(UserName.Text).ToString(), hdnProvAdminUserID.Value, Convert.ToInt32(hdnRegID.Value), true, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        mpeAddAgent.Hide();
        UserName.Text = string.Empty;
        Email.Text = string.Empty;
        ConfirmEmail.Text = string.Empty;
		ViewState["PreviousPage"] = 1;
		GetAgentCount();
        LoadGrid();
    }


    private void ColorPagerFirstPage()
    {
        int PrevPage = Convert.ToInt32(ViewState["PreviousPage"]);
        int NextPage = Convert.ToInt32(ViewState["NextPage"]);
        for (int i = 0; i < rptPaging.Items.Count; i++)
        {
            LinkButton lnkPage = rptPaging.Items[i].FindControl("lbPaging") as LinkButton;

            lnkPage.Enabled = true;
            lnkPage.BackColor = System.Drawing.Color.FromName("#FFFFFF");

            if (Convert.ToInt32(lnkPage.Text) == CurrentPage && CurrentPage != PrevPage && CurrentPage != NextPage)
            {
                lnkPage.Enabled = false;
                lnkPage.BackColor = System.Drawing.Color.FromName("#00FF00");

            }
            else if (CurrentPage == PrevPage && lnkPage != null && i == 0)
            {
                lnkPage.Enabled = false;
                lnkPage.BackColor = System.Drawing.Color.FromName("#00FF00");
            }
            //else if (lnkPage != null && i == 0)
            //{
            //    lnkPage.Enabled = false;
            //    lnkPage.BackColor = System.Drawing.Color.FromName("#00FF00");
            //}
        }
    }
    protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        Session["ItemCommand"] = e.Item.ItemIndex + "@" + e.CommandArgument;
        if ((bool)Session["GridAgentChecked"])
        {
            Mod_UserConfirm.Show();
            Session["PagerClick"] = "ItemCommand";
        }
        else
        {
            Pager_ItemCommand();
        }
    }

    private void Pager_ItemCommand()
    {
        string[] Item = Session["ItemCommand"].ToString().Split('@');

        for (int i = 0; i < rptPaging.Items.Count; i++)
        {
            LinkButton lnkPage = rptPaging.Items[i].FindControl("lbPaging") as LinkButton;
            if (lnkPage != null && i == Convert.ToInt32(Item[0]))
            {
                lnkPage.Enabled = false;
                lnkPage.BackColor = System.Drawing.Color.FromName("#00FF00");
            }
            else
            {
                lnkPage.Enabled = true;
                lnkPage.BackColor = System.Drawing.Color.FromName("#FFFFFF");
            }
        }

        CurrentPage = Convert.ToInt32(Item[1]);
		lblpage.Text = "Page " + (CurrentPage) + " of " + Convert.ToString(ViewState["PageCount"]);

		if (!string.IsNullOrEmpty(lblpage.Text))
			lblpage.Visible = true;

		BindDataIntoRepeater(CurrentPage);
        BindAgentData((Convert.ToInt32(Item[1]) - 1) * 5);//offset
    }

    protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
        if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
    }


    private void BindDataIntoRepeater(int currentPage)
    {
        int AgentsCount;
        if (Session["AgentsCount"] == null)
        {
            AgentsCount = 0;
        }
        else
        {
            AgentsCount = (int)Session["AgentsCount"];
        }
        double totalPageCount = (double)(AgentsCount) / PageSize;
        int pageCount = (int)Math.Ceiling(totalPageCount);
        ViewState["PageCount"] = pageCount;
        ViewState["TotalPages"] = PageCount;
        lblpage.Text = "Page " + (CurrentPage) + " of " + pageCount;

		if (!string.IsNullOrEmpty(lblpage.Text))
			lblpage.Visible= true;

		lbPrevious.Enabled = CurrentPage != 1;
        lbNext.Enabled = CurrentPage < pageCount;
        lbFirst.Enabled = CurrentPage != 1;
        lbLast.Enabled = pageCount != CurrentPage;
        HandlePaging(currentPage, pageCount);
    }
    protected void lbFirst_Click(object sender, EventArgs e)
    {
        if ((bool)Session["GridAgentChecked"])
        {
            Mod_UserConfirm.Show();
            Session["PagerClick"] = "First";
        }
        else
        {
            Pager_First();

        }
    }

    private void Pager_First()
    {
        ViewState["PreviousPage"] = 1;
        CurrentPage = 1;
        ViewState["NextPage"] = CurrentPage + 1;
        BindDataIntoRepeater(CurrentPage);
        BindAgentData(0);//Offset
        ColorPagerFirstPage();
    }

    protected void lbPrevious_Click(object sender, EventArgs e)
    {
        if ((bool)Session["GridAgentChecked"])
        {
            Mod_UserConfirm.Show();
            Session["PagerClick"] = "Previous";

        }
        else
        {
            Pager_Previous();
        }
    }

    private void Pager_Previous()
    {
        int NextPage = Convert.ToInt32(ViewState["NextPage"]);
        int PrevPage = Convert.ToInt32(ViewState["PreviousPage"]);

        if (PrevPage - PageSize > 0)
        {
            //ViewState["NextPage"] = NextPage - 1;
            //CurrentPage = PrevPage - 1;
            //ViewState["PreviousPage"] = CurrentPage;
            ViewState["NextPage"] = CurrentPage;
            CurrentPage = PrevPage;
            ViewState["PreviousPage"] = PrevPage - 1;
        }
        else
        {

            //ViewState["NextPage"] = CurrentPage + 1;
            //CurrentPage = PrevPage - 1;
            //ViewState["PreviousPage"] = CurrentPage;
            ViewState["NextPage"] = CurrentPage ;
            CurrentPage = PrevPage ;
            ViewState["PreviousPage"] = PrevPage - 1;


        }
        BindDataIntoRepeater(CurrentPage);
        //BindAgentData((Convert.ToInt32(ViewState["PreviousPage"]) - 1) * 5);        
        BindAgentData((CurrentPage - 1) * 5);//Offset set to 0
        ColorPagerFirstPage();
    }

    protected void lbNext_Click(object sender, EventArgs e)
    {
        if ((bool)Session["GridAgentChecked"])
        {
            Mod_UserConfirm.Show();
            Session["PagerClick"] = "Next";
        }
        else
        {
            Pager_Next();
        }
    }

    private void Pager_Next()
    {
        int NextPage = Convert.ToInt32(ViewState["NextPage"]);
        int PrevPage = Convert.ToInt32(ViewState["PreviousPage"]);
        int PageCount = Convert.ToInt32(ViewState["PageCount"]);

        if (NextPage + 1 < PageCount)
        {
            ViewState["NextPage"] = NextPage + 1;
            CurrentPage = CurrentPage + 1;;
            ViewState["PreviousPage"] = CurrentPage + 1;
        }
        else
        {
            int Remainder = PageCount % PageSize;

            if (Remainder != 0)
            {
                CurrentPage = CurrentPage + 1;
                ViewState["PreviousPage"] = CurrentPage;
                ViewState["NextPage"] = CurrentPage + 1;
            }
            else
            {

                CurrentPage = CurrentPage + 1;
                ViewState["PreviousPage"] = CurrentPage;
                ViewState["NextPage"] = CurrentPage + 1;
            }
        }


        BindDataIntoRepeater(CurrentPage);
        //BindAgentData((Convert.ToInt32(ViewState["PreviousPage"]) - 1) * 5);
        BindAgentData((CurrentPage - 1) * 5);//Offset set to 0
        ColorPagerFirstPage();
    }

    protected void lbLast_Click(object sender, EventArgs e)
    {
        if ((bool)Session["GridAgentChecked"])
        {
            Mod_UserConfirm.Show();
            Session["PagerClick"] = "Last";
        }
        else
        {
            Pager_Last();
        }
    }

    private void Pager_Last()
    {
        int NextPage = Convert.ToInt32(ViewState["NextPage"]);
        int PrevPage = Convert.ToInt32(ViewState["PreviousPage"]);
        int PageCount = Convert.ToInt32(ViewState["PageCount"]);
        int Remainder = PageCount % PageSize;

        /*if (Remainder != 0)
        {
            CurrentPage = PageCount - Remainder + 1;

        }
        else
        {

            CurrentPage = PageCount - PageSize + 1;
        }*/
		CurrentPage = PageCount;

        ViewState["PreviousPage"] = CurrentPage;
        ViewState["NextPage"] = 1;
        BindDataIntoRepeater(CurrentPage);
        // BindAgentData((Convert.ToInt32(ViewState["PreviousPage"]) - 1) * 5);
        BindAgentData((CurrentPage - 1) * 5);//Offset set to 0
        ColorPagerFirstPage();
    }

    private int CurrentPage
    {
        get
        {
            if (ViewState["CurrentPage"] == null)
            {
                return 0;
            }
            return ((int)ViewState["CurrentPage"]);
        }
        set
        {
            ViewState["CurrentPage"] = value;
        }
    }

    private int NextPage
    {
        get
        {
            if (ViewState["NextPage"] == null)
            {
                return 0;
            }
            return ((int)ViewState["NextPage"]);
        }
        set
        {
            ViewState["NextPage"] = value;
        }
    }

    private int PageCount
    {
        get
        {
            if (ViewState["PageCount"] == null)
            {
                return 0;
            }
            return ((int)ViewState["PageCount"]);
        }
        set
        {
            ViewState["PageCount"] = value;
        }
    }

    private void HandlePaging(int CurrentPage, int pageCount)
    {
        var dt = new DataTable();
        dt.Columns.Add("PageIndex");
        dt.Columns.Add("PageText");
        int pageStart = 1;

        if (pageCount != 1)
            // pageStart = Convert.ToInt32(ViewState["PreviousPage"]);
            pageStart = CurrentPage;

        int Iterations;
        if (CurrentPage + PageSize > pageCount)
        {
            Iterations = PageCount + 1;
            lbPrevious.Enabled = CurrentPage != 1;
            lbNext.Enabled = CurrentPage < PageCount;
            lbFirst.Enabled = CurrentPage != 1;
            lbLast.Enabled = pageCount != CurrentPage;
        }
        else
        {
            Iterations = pageStart + PageSize;
        }
        for (var i = pageStart; i < Iterations; i++)
        {
			if (i <= 0) continue;
            var dr = dt.NewRow();
            dr[0] = i;
            dr[1] = i;
            dt.Rows.Add(dr);
        }

        rptPaging.DataSource = dt;
        rptPaging.DataBind();
    }

    protected void Btn_Yes_Click(object sender, EventArgs e)
    {
        string Pager_Action = Session["PagerClick"].ToString();

        switch (Pager_Action)
        {
            case "ItemCommand":
                Pager_ItemCommand();
                break;
            case "First":
                Pager_First();
                break;
            case "Previous":
                Pager_Previous();
                break;
            case "Next":
                Pager_Next();
                break;

            case "Last":
                Pager_Last();
                break;
        }
        Session["PagerClick"] = null;
        Session["ItemCommand"] = null;
        Session["GridAgentChecked"] = false;
        Mod_UserConfirm.Hide();
    }

    protected void Btn_No_Click(object sender, EventArgs e)
    {
        Session["PagerClick"] = null;
        Session["ItemCommand"] = null;
        Mod_UserConfirm.Hide();
    }
    
    protected void btnSearchAgent_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSeltAgent.Text))
        {
            MembershipUser user = Membership.GetUser(txtSeltAgent.Text);
            if (user == null) //To be verified
            {
                URP03_ERR.Visible = true;
                URP03_ERR.Text = "User ID entered does not exist.<br />";
                return;
            }

            if (!Roles.IsUserInRole(txtSeltAgent.Text, CON.UserRoleType.ProviderAgent))
            {
                URP03_ERR.Visible = true;
                URP03_ERR.Text = "Agent not found.<br />";
                return;
            }
        }
		ViewState["PreviousPage"] = 1;
		GetAgentCount();
        LoadGrid();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {if((CurrentPage== 1) || (CurrentPage== 0)) { ViewState["PreviousPage"] = 1; }
            else 
        { ViewState["PreviousPage"] = Convert.ToInt32(CurrentPage) - 1; }
        
    }
}
