using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for BasePopupControl
/// </summary>
public class BasePopupControl : System.Web.UI.UserControl
{
	public BasePopupControl()
	{
	}

    public virtual void LoadData(DataRow dr)
    {
        // Inherited control will need to override this method.
    }

    public DataTable DataList
    {
        get
        {
            string id = this.UniqueID + "_DataList";
            if (ViewState[id] == null) ViewState[id] = new DataTable();
            return (DataTable)ViewState[id];
        }
        set 
        {
            string id = this.UniqueID + "_DataList";
            ViewState[id] = value; 
        }
    }

    public void GridviewShowNoResultFound<T>(List<T> source, GridView gv)
    {
        // Bind the DataTable which contain a blank row to the GridView
        gv.DataSource = source;
        gv.DataBind();
        // Get the total number of columns in the GridView to know what the Column Span should be
        int columnsCount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();// clear all the cells in the row
        gv.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
        gv.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

        //You can set the styles here
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
    }
}