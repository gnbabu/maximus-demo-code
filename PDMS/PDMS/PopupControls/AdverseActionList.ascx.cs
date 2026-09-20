using System.Data;

public partial class PopupControls_AdverseActionList : System.Web.UI.UserControl
{
	public void LoadData(DataTable dt)
	{
		grdAdverseActions.DataSource = dt;
		grdAdverseActions.DataBind();
	}
}