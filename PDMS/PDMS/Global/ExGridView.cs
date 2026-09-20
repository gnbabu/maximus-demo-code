using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    [
    ToolboxData("<{0}:ExGridView runat=\"server\"> </{0}:ExGridView>")
    ]
    public class ExGridView : GridView
    {

        public ExGridView() { }

        public Unit GridHeight { get; set; }

        private String CalculateWidth()
        {
            string strWidth = "auto";
            if (!this.Width.IsEmpty)
            {
                strWidth = String.Format("{0}{1}", this.Width.Value, ((this.Width.Type == UnitType.Percentage) ? "%" : "px"));
            }
            return strWidth;
        }

        private String CalculateHeight()
        {
            string strHeight = "200px";
            if (!this.GridHeight.IsEmpty)
            {
                strHeight = String.Format("{0}{1}", this.GridHeight.Value, ((this.GridHeight.Type == UnitType.Percentage) ? "%" : "px"));
            }
            return strHeight;
        }

        public virtual int PageCount { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            // render data rows
            writer.Write("<div id='" + ClientID + "_div'  style='" +
                             "padding-bottom:5px;overflow-x:auto;overflow-y:auto;" +

                             "width:" + CalculateWidth() + ";" +
                             "height:" + CalculateHeight() + ";" +
                             "background-color:#FFFFFF;font-size: 8pt;'>");

            //get the pager row and make invisible
            GridViewRow customPager = this.BottomPagerRow;
            if (this.BottomPagerRow != null)
            {
                this.BottomPagerRow.Visible = false;
            }

            base.Render(writer);
            writer.Write("</div>");

            //render pager row

            this.PageCount = 13;

            if (customPager != null && this.PageCount > 0)
            {
                writer.Write("<table  border='0' cellspacing='" + this.CellSpacing.ToString() + "' cellpadding='" + this.CellPadding.ToString() + "' style='width:" + CalculateWidth() + "'>");
                customPager.ApplyStyle(this.PagerStyle);
                customPager.Visible = true;
                customPager.RenderControl(writer);
                writer.Write("</table>");
            }
        }
    }
}