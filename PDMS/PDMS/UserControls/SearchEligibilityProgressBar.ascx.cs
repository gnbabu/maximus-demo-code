using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_SearchEligibilityProgressBarascx : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Refresh page load 
        Refresh();

        if (!IsPostBack)
        {
            //Refresh();
            this.Visible = (HttpContext.Current.User.Identity.IsAuthenticated);

            rrMenu.Width = 450;
            this.WorkflowPage.RegistrationStep = 10001;
            if (Session["StepId"] != null)
            {
                this.WorkflowPage.RegistrationStep = Convert.ToInt32(Session["StepId"]);
                if (RadJumpTo.Items.Any())
                {
                    RadJumpTo.DataBind();
                    RadJumpTo.SelectedValue = Session["StepId"].ToString();
                }
                Session.Remove("StepId");


            }
            //SelectStep();
            RefreshWorkflowPage();
        }
    }

    public void SelectStep()
    {
        int currentStep = this.WorkflowPage.RegistrationStep;
        var rtb = (RadToolBarButton)rrMenu.FindItemByValue(currentStep.ToString());
        hndCurrentItem.Value = rtb != null ? rtb.Value : "";
        if (RadJumpTo.Items.Any())
        {
            try
            {
                if (RadJumpTo.Items.Count > 1)
                {

                    if (RadJumpTo.Items.Any())
                    {

                        RadJumpTo.DataBind();
                        RadJumpTo.SelectedValue = currentStep.ToString();
                        RadJumpTo.Text = rtb.Text;
                    }
                }
            }
            catch (Exception ex)
            { }

        }
    }
    public delegate void RefreshEventHandler(int step);
    public event RefreshEventHandler RefreshEvent;

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }



    protected void rrMenu_ButtonClick(object sender, RadToolBarEventArgs e)
    {
        if (e.Item.Value != string.Empty)
        {
            CheckForDeleteOfAttachments();

            this.WorkflowPage.RegistrationStep = int.Parse(e.Item.Value);
            string jumpto_text = e.Item.Text;
            RadJumpTo.Text = jumpto_text.IndexOf('<') > 0 ? jumpto_text.Substring(0, jumpto_text.IndexOf('<')) : jumpto_text;
            RadJumpTo.SelectedValue = this.WorkflowPage.RegistrationStep.ToString();
        }

        if (e.Item.Value != string.Empty)
            switch (RadJumpTo.SelectedValue)
            {
                case "10002": break;
                default:
                    Helper.PurgeSessionData();
                    break;
            }
        ValidatePageMedicaidId();
        Refresh();
        RefreshWorkflowPage();


    }

    protected void RadJumpTo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        // TODO: EDV The JumpTo is behaving weird. we will come back to it later
        if (!string.IsNullOrEmpty(e.Value))
        {
            //Check if leaving the attachments page
            CheckForDeleteOfAttachments();

            this.WorkflowPage.RegistrationStep = int.Parse(e.Value);
            ValidatePageMedicaidId();
            Refresh();
            RefreshWorkflowPage();

        }
    }

    private void ValidatePageMedicaidId()
    {
        var medicaidId = this.WorkflowPage.MedicaidID;
        if (medicaidId == null)
        {
            //throw new Exception("MedicaidNumber not found");
            Response.Redirect("~/Default.aspx");
            //Process/FinancialProviderInformation.aspx
        }
    }
    protected void RadJumpTo_DataBound(object sender, EventArgs e)
    {
        ((Literal)RadJumpTo.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(RadJumpTo.Items.Count);
    }

    private DataTable getJumpToTable()
    {
        DataTable jumpToTable = new DataTable();
        jumpToTable.Columns.Add("ID");
        jumpToTable.Columns.Add("Value");
        jumpToTable.Columns.Add("Text");
        jumpToTable.Columns.Add("Icon");
        jumpToTable.Columns.Add("Status");
        return jumpToTable;
    }

    public void Refresh()
    {
        rrMenu.Items.Clear();
        AddToolBarButtons();
    }

    private List<RegistrationNode> LoadPageNodes()
    {
        int seq = 0;
        string displayMenu = string.Empty;
        int providerStatusId = 0;
        int IsRequired = 0;
        int statusId = 0;
        bool isVisible;
        DataTable dtSectionData = LoadSectionData();
        this.WorkflowPage.RegistrationNodes = new Dictionary<int, RegistrationNode>();
        foreach (DataRow dr in dtSectionData.Rows)
        {
            seq++;
            displayMenu = dr["SECTION_DISPLAY_NAME"].ToString();
            providerStatusId = Convert.ToInt32(dr["REG_PROVIDER_STATUS_TYPE_ID"].ToString());
            isVisible = Convert.ToBoolean(dr["IS_VISIBLE"].ToString());
            if (isVisible)
            {
                this.WorkflowPage.RegistrationNodes.Add(Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), new RegistrationNode(seq, displayMenu, Convert.ToInt32(dr["REG_SECTION_TYPE_ID"].ToString()), statusId, IsRequired, providerStatusId, 1, 0));
            }
        }
        return this.WorkflowPage.RegistrationNodes.Select(s => s.Value).ToList();
    }

    private DataTable LoadSectionData()
    {
        string MedicaidNumber = this.WorkflowPage.MedicaidID;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("MEDICAID_ID", MedicaidNumber);
        parms.Add("USER_ID", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectFINANCIAL_PAGES_GETALL", parms);

        return ds.Tables[0];
    }

    private void AddToolBarButtons()
    {
        int ctr = 0;
        List<RegistrationNode> nodes = LoadPageNodes();
        DataTable jumpToTable = getJumpToTable();
        foreach (RegistrationNode node in nodes)
        {
            RadToolBarButton rtb = new RadToolBarButton();
            Label lblRequired = new Label();
            string statusType = "";
            bool isEditable = true;

            lblRequired.Text = "<span style='color:red'>*</span>";
            string buttonText = node.MenuPath;
            if (node.MenuPath == "Affiliations")
                if (Registration.IsEPDProvider(this.WorkflowPage.RegistrationId) || Registration.IsHomeHealthProvider(this.WorkflowPage.RegistrationId))
                    buttonText = "PCA Aides";
                else if (Registration.CanIndividualAddPA(this.WorkflowPage.RegistrationId))
                    buttonText = "Physician Assistants";

            if (node.MenuPath == "CDS Number" && Registration.IsCDSPageRequired(this.WorkflowPage.RegistrationId))
                node.IsRequired = 1;

            rtb.Text = buttonText + (node.IsRequired == 1 ? lblRequired.Text : "");
            rtb.Value = node.Step.ToString();

            //OHPNM-16560-3B - Standardize Progressbar Icons
            //string ImgUrl = "~/images/" + node.MenuPath.Replace(" ", "") + ".png";
            //if (File.Exists(Server.MapPath(ImgUrl)))
            //    rtb.ImageUrl = ImgUrl;
            //else
            rtb.ImageUrl = "~/images/ProviderInformation.png";

            rtb.ImagePosition = ToolBarImagePosition.AboveText;
            rtb.CheckOnClick = true;
            rtb.CssClass = GetCssName(node.StatusId, statusType);
            rtb.Enabled = isEditable;
            rtb.PostBack = true;
            rtb.EnableViewState = false;
            rtb.Attributes.Add("OnClick", "ItemClick('" + rtb.Value + "');");

            //if (node.MenuPath == "Correspondence" || node.MenuPath == "Search-RA")
            //{
            if ((string.Equals(AppSettings.Get("ShowPAIconPNM3BPA"), "1") && (node.MenuPath == "Submit PA" || node.MenuPath == "Search PA"))
                || (string.Equals(AppSettings.Get("ShowHospiceIconPNM3B"), "1") && (node.MenuPath == "Hospice Enrollment"))
                || (string.Equals(AppSettings.Get("ShowProviderFinancialIconPNM3B"), "1") && (node.MenuPath == "Provider Financial"))
                || (string.Equals(AppSettings.Get("ShowUploadAttachmentIconPNM3BPA"), "1") && (node.MenuPath == "Upload Attachments"))
                || (string.Equals(AppSettings.Get("ShowMemberEligibilityIconPNM3B"), "1") && (node.MenuPath == "Search Eligibility"))
                || (string.Equals(AppSettings.Get("ShowMemberEligibilityIconPNM3B"), "2") && (node.MenuPath == "Search Eligibility"))
                || (string.Equals(AppSettings.Get("ShowClaimsIconPNM3B"), "1") && ((node.MenuPath == "Submit Claim") || (node.MenuPath == "Search Claim"))
                || node.MenuPath == "Correspondence" || node.MenuPath == "Search-RA"))

            {
                rrMenu.Items.Add(rtb);

                jumpToTable.Rows.Add(new String[] { node.Sequence.ToString(), rtb.Value, rtb.Text, rtb.ImageUrl, GetImageUrl(node.StatusId, isEditable, statusType) });
                ctr++;
            }


            // }

            if (ctr < nodes.Count)
            {
                RadToolBarButton rtbArrow = new RadToolBarButton();
                rtbArrow.Text = "\t\t\t";
                rtbArrow.HoveredCssClass = rtbArrow.CheckedCssClass = "noHover";
                rtbArrow.ClickedCssClass = rtbArrow.FocusedCssClass = "noHover";
                rtbArrow.EnableViewState = false;
                rtbArrow.PostBack = false;
                rrMenu.Items.Add(rtbArrow);
            }
        }
        foreach (RadComboBoxItem item in RadJumpTo.Items)
        {
            item.Attributes.Add("OnClick", "ItemClick('" + item.Text + "','" + item.Value + "');");
        }
        RadJumpTo.DataSource = jumpToTable;
        RadJumpTo.DataBind();
        RadJumpTo.EnableViewState = false;
    }

    private string GetCssName(int statusId, string statusType)
    {
        string rtn = "";

        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
        {
            if (statusId == CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = "completebg";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider) rtn = "inProcessbg";
            if (statusId == CON.RegistrationProviderStatusTypeId.Modified && statusType == CON.StatusType.RegistrationProviderStatusTypeId) rtn = "needAttentionbg";
        }
        else
        {
            if (statusId == CON.RegistrationProviderStatusTypeId.Complete) rtn = "completebg";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider && Helper.RegistrationInReturnedToProvider(this.WorkflowPage.RegistrationId)) rtn = "inProcessbg";
            else if (statusId == CON.RegistrationProviderStatusTypeId.Modified) rtn = "needAttentionbg";
        }
        return rtn;
    }

    private string GetImageUrl(int statusId, bool isEditable, string statusType)
    {
        string rtn = "";

        if (!isEditable)
        {
            rtn = "~/Images/icon-circle-slash.png";
        }
        else if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
        {
            if (statusId == CON.RegistrationProviderServicesStatusTypeId.Approved) rtn = "~/Images/StepCheck.png";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider) rtn = "~/Images/InProcess.png";
            if (statusId == CON.RegistrationProviderStatusTypeId.Modified && statusType == CON.StatusType.RegistrationProviderStatusTypeId) rtn = "~/Images/bullet-red.png";
        }
        else
        {
            if (statusId == CON.RegistrationProviderStatusTypeId.Complete) rtn = "~/Images/StepCheck.png";
            else if (statusId == CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider && Helper.RegistrationInReturnedToProvider(this.WorkflowPage.RegistrationId)) rtn = "~/Images/InProcess.png";
            else if (statusId == CON.RegistrationProviderStatusTypeId.Modified) rtn = "~/Images/bullet-red.png";
        }


        return rtn;
    }


    protected void RadJumpTo_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        string jumpto_text = ((DataRowView)e.Item.DataItem)["Text"].ToString();
        e.Item.Text = jumpto_text.IndexOf('<') > 0 ? jumpto_text.Substring(0, jumpto_text.IndexOf('<')) : jumpto_text;
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Value"].ToString();
    }

    public void RefreshWorkflowPage(bool forceRedirect = false)
    {
        // There is no force redirect. It is always refresh
        if (RefreshEvent != null) RefreshEvent(this.WorkflowPage.RegistrationStep);
    }

    private void CheckForDeleteOfAttachments()
    {
        //Check if leaving the PA, Submit Clamis or attachments page
        if (WorkflowPage.RegistrationStep == 10002 || WorkflowPage.RegistrationStep == 10005 || WorkflowPage.RegistrationStep == 10015)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.DeleteAttachmentsForMedicaidID(WorkflowPage.MedicaidID);
        }
        return;
    }
}