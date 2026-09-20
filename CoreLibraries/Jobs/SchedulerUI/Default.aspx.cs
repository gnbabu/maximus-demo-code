using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SchedulerUI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Scheduler.Business.Scheduler sc = new Scheduler.Business.Scheduler();
            RadGrid1.DataSource = sc.GetSchedules();
        }

        protected void TriggerTable_DataBinding(object sender, EventArgs e)
        {

        }
        protected void RadGrid1_DetailTableDataBind(object sender, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            Scheduler.Business.Scheduler sc = new Scheduler.Business.Scheduler();

            string name = dataItem["Name"].Text;
            string group = dataItem["Group"].Text;
            e.DetailTableView.DataSource = sc.GetSchedule(name, group).TriggerList;
            //e.DetailTableView.DataBind();
        }

        protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if ((e.CommandSource is LinkButton) && ((LinkButton)e.CommandSource).Text == "Run Now")
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                Scheduler.Business.Scheduler sc = new Scheduler.Business.Scheduler();

                string name = dataItem["Name"].Text;
                string group = dataItem["Group"].Text;
                sc.TriggerJob(name, group);
            }

        }
        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            //RadGrid1.DataBind();

        }
    }
}