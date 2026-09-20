using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_CEOUserAgentAccountsAdministration : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadGrid();
    }
    protected void LoadGrid()
    {
        
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        grdSecondaryUserFacility.Columns.Clear();
        grdDeactivateUser.Columns.Clear();
        grdSecondaryUserContract.Columns.Clear();

        DataSet FacilityDS = psc.SelectAgentFacilities_ContractsByCEOUser(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        if (!Helper.HasRows(FacilityDS))
        {
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "No Agents are mapped to this user.<br />";
            return;
        }
        BoundField userNameColumn = new BoundField();
        userNameColumn.HeaderText = "Secondary User-Facility ID";   // JIRA 2608
        userNameColumn.DataField = "DD_FACILITY_NUMBER";
        userNameColumn.HeaderStyle.Width = 300;
        grdSecondaryUserFacility.Columns.Add(userNameColumn);

        AddTemplateFieldToGridView(grdSecondaryUserFacility, "DD_FACILITY_NUMBER");

        var ownerUserName = HttpContext.Current.User.Identity.Name;

        // Leave out the columns with DD_FACILITY_NUMBER, AGENT_SUB_ROLES_ID and the rest of them should have the checkboxes
        for (int columnNum = 2; columnNum < FacilityDS.Tables[1].Columns.Count; columnNum++)
        {
            var columnName = FacilityDS.Tables[1].Columns[columnNum].ColumnName;
            //if (columnName == ownerUserName)
            //{
            //    columnName += "*";
            //    FacilityDS.Tables[1].Columns[columnNum].ColumnName += "*";
            //}
            TemplateField bfield = new TemplateField();
            bfield.HeaderText = columnName;
            //Initalize the DataField value.
            bfield.HeaderTemplate = new GridViewCheckBoxTemplate(ListItemType.Header, columnName);
            //Initialize the HeaderText field value.
            bfield.ItemTemplate = new GridViewCheckBoxTemplate(ListItemType.Item, columnName);

            grdSecondaryUserFacility.Columns.Add(bfield);
        }

        BoundField userNameColumn1 = new BoundField();
        userNameColumn1.HeaderText = "Secondary User-Contract ID";  // JIRA 2608
        userNameColumn1.DataField = "DD_CONTRACT_NUMBER";
        userNameColumn1.HeaderStyle.Width = 300;
        grdSecondaryUserContract.Columns.Add(userNameColumn1);

        AddTemplateFieldToGridView(grdSecondaryUserContract, "DD_CONTRACT_NUMBER");
        // Leave out the columns with DD_CONTRACT_NUMBER, AGENT_SUB_ROLES_ID and the rest of them should have the checkboxes
        for (int columnNum = 2; columnNum < FacilityDS.Tables[2].Columns.Count; columnNum++)
        {
            var columnName = FacilityDS.Tables[2].Columns[columnNum].ColumnName;
          
            TemplateField bfield = new TemplateField();
            bfield.HeaderText = columnName;
            //Initalize the DataField value.
            bfield.HeaderTemplate = new GridViewCheckBoxTemplate(ListItemType.Header, columnName);
            //Initialize the HeaderText field value.
            bfield.ItemTemplate = new GridViewCheckBoxTemplate(ListItemType.Item, columnName);

            grdSecondaryUserContract.Columns.Add(bfield);
        }

        BoundField pageNameForDeactivateUser = new BoundField();
        pageNameForDeactivateUser.HeaderText = "Action";
        pageNameForDeactivateUser.DataField = "Action";
        pageNameForDeactivateUser.HeaderStyle.Width = 300;
        grdDeactivateUser.Columns.Add(pageNameForDeactivateUser);

        for (int columnNum = 1; columnNum < FacilityDS.Tables[0].Columns.Count; columnNum++)
        {
            var columnName = FacilityDS.Tables[0].Columns[columnNum].ColumnName;
           
            TemplateField bfield = new TemplateField();
            bfield.HeaderText = columnName;
            //Initalize the DataField value.
            bfield.HeaderTemplate = new GridViewButtonTemplate(ListItemType.Header, columnName, new EventHandler(grdDeactivateUser_ButtonClick));
            //Initialize the HeaderText field value.
            bfield.ItemTemplate = new GridViewButtonTemplate(ListItemType.Item, columnName, new EventHandler(grdDeactivateUser_ButtonClick));
            grdDeactivateUser.Columns.Add(bfield);
            
        }

        grdDeactivateUser.DataSource = FacilityDS.Tables[0];
        grdDeactivateUser.DataBind();

        grdSecondaryUserFacility.DataSource = FacilityDS.Tables[1];
        grdSecondaryUserFacility.DataBind();

        grdSecondaryUserContract.DataSource = FacilityDS.Tables[2];
        grdSecondaryUserContract.DataBind();
    }

    private void grdDeactivateUser_ButtonClick(object sender, EventArgs e)
    {
        Button deactivateUser = (Button)sender;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.DeleteProviderAgentMapping(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Helper.GetUserId(deactivateUser.CommandArgument));
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
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Font.Bold = true;
            }
            e.Row.Font.Bold = true;
        }

    }
    protected void grdSecondaryUserFacility_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "UserID")
                e.Row.Visible = false;

            if (e.Row.Cells[0].Text == "Facility ID")
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != "Facility ID" && cell.Controls[0].Visible)
                        cell.Controls[0].Visible = false;
                }
                e.Row.Font.Bold = true;
            }
        }
    }
    protected void grdSecondaryUserContract_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text == "UserID")
                e.Row.Visible = false;

            if (e.Row.Cells[0].Text == "Contract ID")
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Text != "Contract ID" && cell.Controls[0].Visible)
                        cell.Controls[0].Visible = false;
                }
                e.Row.Font.Bold = true;
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable("AGENT_FACILITY_CONTRACT_TABLE");
        DataRow dr;
        dt.Columns.Add("PROVIDER_ADMIN_USER_ID", typeof(string));
        dt.Columns.Add("AGENT_USER_ID", typeof(string));
        dt.Columns.Add("DD_FACILITY_NUMBER", typeof(string));
        dt.Columns.Add("DD_CONTRACT_NUMBER", typeof(string));
        dt.Columns.Add("IS_SELECTED", typeof(int));

        foreach (DataControlField column in grdSecondaryUserFacility.Columns)
        {

            string userName = string.Empty;
            int columnIndex = grdSecondaryUserFacility.Columns.IndexOf(column);

            if (columnIndex == 0 || columnIndex == 1)
                continue;


            if (grdSecondaryUserFacility.Rows.Count > 0)
            {
                foreach (GridViewRow row in grdSecondaryUserFacility.Rows)
                {

                    CheckBox checkBox = (CheckBox)row.Cells[columnIndex].FindControl("permCheckBox_" + column.HeaderText);
                    Label label = (Label)row.Cells[1].FindControl("permuserLabel" + "DD_FACILITY_NUMBER");

                    var columnName = column.HeaderText.Replace("*", "");
                    if (columnName == "DD_FACILITY_NUMBER" || label.Text == "UserID" || label.Text == "Facility ID")
                        continue; // this is the hidden row for the user


                    var userId = Membership.GetUser(columnName);

                    dr = dt.NewRow();

                    dr["PROVIDER_ADMIN_USER_ID"] = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                    dr["AGENT_USER_ID"] = userId.ProviderUserKey.ToString();
                    dr["DD_FACILITY_NUMBER"] = label.Text;

                    if (checkBox.Checked)
                    {
                        dr["IS_SELECTED"] = 1;
                    }
                    else
                    {
                        dr["IS_SELECTED"] = 0;
                    }

                    dt.Rows.Add(dr);

                }
            }
        }
        foreach (DataControlField column in grdSecondaryUserContract.Columns)
        {
            string userName = string.Empty;
            int columnIndex = grdSecondaryUserContract.Columns.IndexOf(column);

            if (columnIndex == 0 || columnIndex == 1)
                continue;

            var columnName = column.HeaderText.Replace("*", "");
            var userId = Membership.GetUser(columnName);

            if (grdSecondaryUserContract.Rows.Count > 0)
            {

                foreach (GridViewRow row in grdSecondaryUserContract.Rows)
                {
                    CheckBox checkBox = (CheckBox)row.Cells[columnIndex].FindControl("permCheckBox_" + column.HeaderText);
                    Label label = (Label)row.Cells[1].FindControl("permuserLabel" + "DD_CONTRACT_NUMBER");
                    if (columnName == "DD_CONTRACT_NUMBER" || label.Text == "UserID" || label.Text == "Contract ID")
                        continue; // this is the hidden row for the user
                    dr = dt.NewRow();

                    dr["PROVIDER_ADMIN_USER_ID"] = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                    dr["AGENT_USER_ID"] = userId.ProviderUserKey.ToString();
                    dr["DD_CONTRACT_NUMBER"] = label.Text;
                    if (checkBox.Checked)
                    {
                        dr["IS_SELECTED"] = 1;
                    }
                    else
                    {
                        dr["IS_SELECTED"] = 0;
                    }
                    dt.Rows.Add(dr);


                }
            }
        }

        Dictionary<string, object> parms = new Dictionary<string, object>();
        parms.Add("AGENT_FACILITY_CONTRACT_TABLE", dt);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.SaveSecondaryUserFacility_ContractByCEOUser(parms);
        LoadGrid();
    }

    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        mpeAddAgent.Show();
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Process/ProviderHomeNew.aspx");
    }

    protected void Validate_UserNameExists(object sender, ServerValidateEventArgs e)
    {
        bool isSecUser = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetUserAccountInformation(Helper.GetUserId(UserName.Text).ToString());
        if(Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                isSecUser = Helper.GetString("USER_TYPE", row) == "DODD Agent" ? true : false; 
            }
        }
        
        e.IsValid = Membership.GetUser(UserName.Text) != null && Roles.IsUserInRole(UserName.Text, CON.UserRole.ProviderAgent) && isSecUser;

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
                isValid = false;
            }
        }
        return isValid;
    }
    protected void btnSaveAddAgent_Click(object sender, EventArgs e)
    {
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
        psc.InsertNewProviderAgent(Helper.GetUserId(UserName.Text).ToString(), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), 0, true, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        mpeAddAgent.Hide();
        LoadGrid();
    }
}