using System;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

namespace Process
{
	public partial class Process_SendMail : Page
	{
		
		protected void Page_PreInit(object sender, EventArgs e)
		{
			if (Helper.IsModern())
			{
				Page.Theme = "Modernization";
			}
			else
			{
				Page.Theme = "Default";
			}
			//added Page Title for 508 complaint
			Page.Title = Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name) ? "Home" : "Dashboard";
		}

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
			}
		}

		protected void RadTabStrip1_TabClick(object sender, RadTabStripEventArgs e)
		{
			EmailQueueListing.GetItemsInQueue();
		}
	}
}