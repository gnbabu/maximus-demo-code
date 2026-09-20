using System;
using System.Web;
using System.Web.UI;

public partial class UserControls_AdverseActionHeader : System.Web.UI.UserControl
{
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

	#region Properties

	public string Title
	{
		get
		{
			return lblTitle.Text;
		}
		set
		{
			lblTitle.Text = value;
		}
	}

	public string AdverseActionTitle
	{
		get
		{
			return lblPopUpTitle.Text;
		}
		set
		{
			lblPopUpTitle.Text = value;
		}
	}

	
	public bool CreateAdverseActionButtonVisible
	{
		get
		{
			return btnCreateAdverseAction.Visible;
		}
		set
		{
			btnCreateAdverseAction.Visible = value;
		}
	}

	public int ScreeningActivityID
	{
		get
		{
			if (ViewState["ScreeningActivityID"] == null)
				ViewState["ScreeningActivityID"] = -1;

			return (int)ViewState["ScreeningActivityID"];
		}
		set
		{
			ViewState["ScreeningActivityID"] = value;
		}
	}

    public  string  AdverseActionText
    {
        get 
        {
            return this.ucAdverseAction.CommentTextBoxText;
        }
    }

	#endregion

	#region Events

	public delegate void CreateAdverseActionEventHandler(PopupControls_AdverseAction.CreateAdverseActionEventArgs args);
	public event CreateAdverseActionEventHandler CreateAdverseAction;

	#endregion

	protected void Page_Load(object sender, EventArgs e)
    {

	}

	#region Event Handlers

	protected void btnCreateAdverseAction_Click(object sender, EventArgs e)	
	{
        ucAdverseAction.Focus();
        mpe.Show();
        ucAdverseAction.Clear();
        ScriptManager.RegisterStartupScript(this,
                                                this.GetType(),
                                                "FocusScript",
                                                "setTimeout(function(){$get('" + ucAdverseAction.CommentTextBoxClientID + "').focus();}, 100);",
                                                true);
	}

	protected void ucAdverseAction_CreateAdverseActionEvent(PopupControls_AdverseAction.CreateAdverseActionEventArgs args)
	{
		if (ScreeningActivityID > 0)
		{
			svc.InsertScreeningAdverseAction(ScreeningActivityID, args.Description, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

			if (CreateAdverseAction != null)
			{
				CreateAdverseAction(args);
			}
		}
	}

	#endregion
}