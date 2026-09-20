using System;

public partial class PopupControls_AdverseAction : System.Web.UI.UserControl
{

	public class CreateAdverseActionEventArgs : EventArgs
	{
		public string Description { get; set; }

		public CreateAdverseActionEventArgs(string description)
			: base()
		{
			Description = description;
		}
	}

	#region Events

	public delegate void CreateAdverseActionEventHandler(CreateAdverseActionEventArgs args);
	public event CreateAdverseActionEventHandler CreateAdverseActionEvent;


	#endregion

	#region Properties

	public string CommentTextBoxClientID
	{
		get
		{
			return txtComments.ClientID;
		}
	}

    public string CommentTextBoxText
    {
        get
        {
            return txtComments.Text;
        }
    }

	#endregion

	protected void Page_Load(object sender, EventArgs e)
    {

    }
	protected void btnSave_Click(object sender, EventArgs e)
	{
		if (CreateAdverseActionEvent != null)
		{
			CreateAdverseActionEvent(new CreateAdverseActionEventArgs(txtComments.Text.Trim()));
		}
	}
	protected void btnCancel_Click(object sender, EventArgs e)
	{

	}

	public override void Focus()
	{
		txtComments.Focus();
	}

	public void Clear()
	{
		txtComments.Text = string.Empty;
	}
}