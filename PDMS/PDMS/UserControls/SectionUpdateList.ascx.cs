using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_SectionUpdateList : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.sectionName.Text = Name;
        this.sectionIcon.ImageUrl = IconSrc;
    }

    public string Name { get; set; }
    public string IconSrc { get; set; }
    public DataTable SectionList { get; set; }

    public override void DataBind()  //LoadData()
    {
        secListGrid.DataSource = SectionList ;
        secListGrid.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        var argument = ((Button)sender).CommandArgument;

        (this.Page as RegistrationProvider).RegistrationId = this.WorkflowPage.RegistrationId;
        (this.Page as RegistrationProvider).IsReadOnly = false;
        (this.Page as RegistrationProvider).RegistrationStep = int.Parse(argument);
    }

    protected void secListGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dataRow = (DataRow)((DataRowView)e.Row.DataItem).Row;
            if (dataRow["REG_PROVIDER_STATUS_TYPE_ID"].ToString() != string.Empty && Convert.ToInt32(dataRow["REG_PROVIDER_STATUS_TYPE_ID"].ToString()) == CON.RegistrationProviderStatusTypeId.Modified)
            {
                var image = (Image)e.Row.FindControl("statusImg");
                image.ImageUrl = "~/Images/Green_Circle_Checkbox.png";
            }
            else if (dataRow["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"].ToString() != string.Empty && Convert.ToInt32(dataRow["REG_PROVIDER_SERVICES_STATUS_TYPE_ID"].ToString()) == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider && Helper.RegistrationInReturnedToProvider(this.WorkflowPage.RegistrationId))
            {
                var image = (Image)e.Row.FindControl("statusImg");
                image.ImageUrl = "~/Images/InProcess_Circle.png";
            }

            // SAM537 Suspended provider can start ODM update and only change primary contact addr, billing addr, correspondence addr,1099 address and home office addr
            bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));
            if (enableCR537 && this.WorkflowPage.IsSuspendedProvider )
            {
                var button = (Button)e.Row.FindControl("btnUpdate");
                if(dataRow["SECTION_DISPLAY_NAME"].ToString() == "Primary Contact Information" || dataRow["SECTION_DISPLAY_NAME"].ToString() == "Billing & Payment Address"
                    || dataRow["SECTION_DISPLAY_NAME"].ToString() == "Correspondence Address" || dataRow["SECTION_DISPLAY_NAME"].ToString() == "1099 Address"
                    || dataRow["SECTION_DISPLAY_NAME"].ToString() == "Home Office Address")
                {
                    button.Enabled = true;
                }
                else
                {
                    button.Enabled = false;
                }
                
            }
        }
    }
    protected void secListGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
}