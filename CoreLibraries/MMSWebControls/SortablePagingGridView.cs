using System;
using System.Web.UI.WebControls;

namespace MMSWebControls
{
    public class SortablePagingGridView : GridView
    {
        #region Events

        public delegate void SelectionClearedHandler(object sender, EventArgs e);
        public event SelectionClearedHandler SelectionCleared;

        #endregion


        public SortablePagingGridView()
            : base()
        {
            AllowPaging = true;
            AllowSorting = true;
            PageSize = 15;
            CssClass = "gridView";
            HeaderStyle.CssClass = "gridViewHeader";
            RowStyle.CssClass = "gridViewRow";
            AlternatingRowStyle.CssClass = "gridViewAltRow";
            PagerStyle.CssClass = "gridViewPager";
            EmptyDataRowStyle.CssClass = "gridViewEmptyRow";
            PagerSettings.Mode = PagerButtons.NumericFirstLast;
            PagerSettings.Visible = true;
            ShowFooter = false;
            SortedAscendingHeaderStyle.CssClass = "sort-asc";  //only works when attached to a datasource
            SortedDescendingHeaderStyle.CssClass = "sort-desc";
            SelectedRowStyle.BackColor = System.Drawing.Color.Yellow;
            SelectedRowStyle.Font.Bold = false;
        }

        #region Private Members

        private const string _virtualCountItem = "bg_vitemCount";
        private const string _sortColumn = "bg_sortColumn";
        private const string _sortDirection = "bg_sortDirection";
        private const string _currentPageIndex = "bg_pageIndex";
        #endregion

        #region Properties

        private const string SORT_ASC_CSSNAME = "sort-asc";
        private const string SORT_DESC_CSSNAME = "sort-desc";
        private const string SORT_ASC_DISABLED_CSSNAME = "sort-asc-disabled";
        private const string SORT_DESC_DISABLED_CSSNAME = "sort-desc-disabled";

        public int VirtualItemCount
        {
            get
            {
                if (ViewState[_virtualCountItem] == null)
                    ViewState[_virtualCountItem] = -1;
                return Convert.ToInt32(ViewState[_virtualCountItem]);
            }
            set
            {
                ViewState[_virtualCountItem] = value;
            }
        }

        public string GridViewSortColumn
        {
            get
            {
                if (ViewState[_sortColumn] == null)
                    ViewState[_sortColumn] = string.Empty;
                return ViewState[_sortColumn].ToString();
            }
            set
            {
                if (ViewState[_sortColumn] == null || !ViewState[_sortColumn].Equals(value))
                    GridViewSortDirection = SortDirection.Ascending;
                ViewState[_sortColumn] = value;
            }
        }


        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState[_sortDirection] == null)
                    ViewState[_sortDirection] = SortDirection.Ascending;
                return (SortDirection)ViewState[_sortDirection];
            }
            set
            {
                ViewState[_sortDirection] = value;
            }
        }

        public int CurrentRowIndex
        {
            get
            {
                return CurrentPageIndex * PageSize; //0 is ok, equivalent to initial pull
            }

        }

        public int CurrentPageIndex
        {
            get
            {
                if (ViewState[_currentPageIndex] == null)
                    ViewState[_currentPageIndex] = 0;
                return Convert.ToInt32(ViewState[_currentPageIndex]);
            }
            set
            {
                ViewState[_currentPageIndex] = value;
            }
        }

        public int PageIndexCount
        {
            get
            {
                if (ViewState["PageCount"] == null)
                    ViewState["PageCount"] = 1;
                return Convert.ToInt32(ViewState["PageCount"]);
            }
            set
            {
                ViewState["PageCount"] = value;
            }
        }

        private bool CustomPaging
        {
            get { return (VirtualItemCount != -1); }
        }
        #endregion

        #region Public Methods


        public void ClearSelection()
        {
            this.SelectedIndex = -1;

            if (SelectionCleared != null)
            {
                SelectionCleared(this, new EventArgs());
            }
        }


        #endregion

        #region Overriding the parent methods
        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;
                // store the page index so we don't lose it in the databind event
                CurrentPageIndex = PageIndex;
            }
        }


        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            // This method is called to initialise the pager on the grid. We intercepted this and override
            // the values of pagedDataSource to achieve the custom paging using the default pager supplied
            pagedDataSource.AllowCustomPaging = true;
            pagedDataSource.VirtualCount = VirtualItemCount;
            pagedDataSource.CurrentPageIndex = CurrentPageIndex;

            base.InitializePager(row, columnSpan, pagedDataSource);
        }

        protected override void OnSorting(GridViewSortEventArgs e)
        {
            //Store the direction to find out if next sort should be asc or desc
            SortDirection direction = SortDirection.Ascending;
            if (GridViewSortColumn != null && (SortDirection)GridViewSortDirection == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
            }

            // HeaderRow.Cells[GetColumnIndex(e.SortExpression)].CssClass = direction == SortDirection.Ascending ? SORT_ASC_CSSNAME : SORT_DESC_CSSNAME;

            GridViewSortDirection = direction;
            GridViewSortColumn = e.SortExpression;

            ClearSelection();
            //init current page index
            CurrentPageIndex = 0;
            base.OnSorting(e);
        }

        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            if (Page.Request.Form.Get("__EVENTARGUMENT").Equals("Page$Last"))
            {
                this.CurrentPageIndex = PageIndexCount - 1;
                this.PageIndex = PageIndexCount - 1;
            }
            else
            {
                this.CurrentPageIndex = e.NewPageIndex;
                this.PageIndex = e.NewPageIndex;                
            }
            ClearSelection();
            base.OnPageIndexChanging(e);
        }

        //protected override object SaveViewState()
        //{
        //    return base.SaveViewState();
        //}

        //protected override void LoadViewState(object savedState)
        //{
        //    base.LoadViewState(savedState);
        //}


        #endregion

        #region Private Methods
        private int GetColumnIndex(string SortExpression)
        {
            int i = 0;
            foreach (DataControlField c in Columns)
            {
                if (c.SortExpression == SortExpression)
                    break;
                i++;
            }
            return i;
        }
        #endregion

    }
}
