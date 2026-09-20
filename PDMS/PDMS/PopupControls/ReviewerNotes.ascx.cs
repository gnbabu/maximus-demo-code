using System;
using System.Data;

public partial class PopupControls_ReviewerNotes : System.Web.UI.UserControl
{
    public DataTable dtReviewer = new DataTable();
    public DataTable dsReviewNotes
    {
        get
        {
            if (!Helper.HasRows(dtReviewer))
            {
                if (Helper.HasRows(GetReviewerNotes()))
                {
                    return dtReviewer;
                }
            }
            return dtReviewer;
        }
        set
        {
            if (value != null)
            {
                dtReviewer = value;
                SetReviewNotes(dtReviewer);
            }
        }
    }

    public string ICNNumber = string.Empty;
    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ICNNumber))
                return ICNNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ICNNumber = value.Trim();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        GetReviewerNotes();
    }
    protected DataTable GetReviewerNotes()
    {
        if (dtReviewer != null && dtReviewer.Rows.Count > 0)
        {
            gvRevNotesProvider.DataSource = dtReviewer;
            gvRevNotesProvider.DataBind();
        }
        return dtReviewer;
    }

    private void SetReviewNotes(DataTable dt)
    {
        gvRevNotesProvider.DataSource = dt;
        gvRevNotesProvider.DataBind();
    }

    public void ClearGrid()
    {
        gvRevNotesProvider.DataSource = null;
        gvRevNotesProvider.DataBind();
    }
}