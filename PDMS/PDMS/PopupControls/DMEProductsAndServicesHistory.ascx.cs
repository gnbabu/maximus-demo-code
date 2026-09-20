using System.Data;

public partial class PopupControls_DMEProductsAndServicesHistory : System.Web.UI.UserControl
{
    public void LoadData(DataTable dt)
    {
        gvProductsAndServicesHistory.DataSource = dt;
        gvProductsAndServicesHistory.DataBind();
    }

}