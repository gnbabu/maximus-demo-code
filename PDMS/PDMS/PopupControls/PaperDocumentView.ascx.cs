using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Views_PaperDocumentView : System.Web.UI.UserControl, IPaperRequestDocumentView
{
    public delegate void ErrorEventHandler(Dictionary<string, string> lstErrors);
    public event ErrorEventHandler ErrorEvent;
    
    public int PaperRequestQueueID
    {
        get
        {
            return ViewState["PaperRequestQueueID"] == null ? 0 : Convert.ToInt32(ViewState["PaperRequestQueueID"]);
        }
        set
        {
            ViewState["PaperRequestQueueID"] = value;
        }
    }

    private string TaxID
    {
        get;
        set;
    }

    private PaperRequestDocumentPresenter _presenter;

    public PaperRequestDocumentPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new PaperRequestDocumentPresenter(this);
            }

            return _presenter;
        }
    }

    public PaperRequestDocumentData Model { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucUploadDocument.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        this.ucUploadDocument.RenameFileMethod += new UserControls_UploadDocument.RenameFile(ucUploadDocument_RenameFileMethod);
        this.ucUploadDocument.SuccessEvent += new UserControls_UploadDocument.SuccessEventHandler(ucUploadDocument_SuccessEvent);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly)) ucUploadDocument.SetUploadButtonEnabled = false;
    }

    public void InitView(int paperRequestQueueID)
    {
        PaperRequestQueueID = paperRequestQueueID;
        presenter.Init(PaperRequestQueueID);
    }

    #region Presenter Events

    public void SetMyDocuments(DataSet ds)
    {
        this.ucUploadDocument.LoadPaperDocs(ds);
    }

    public void SetInsertSuccess()
    {
        presenter.RequestMyDocuments();
    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            if (ErrorEvent != null)
                ErrorEvent(presenter.ErrorList);
        }
    }
    #endregion

    #region Private Methods

    private void InitModel()
    {
        if (Model == null)
        {
            presenter.Init();
            Model.PaperRequestQueueID = this.PaperRequestQueueID;
        }
    }

    private string ucUploadDocument_RenameFileMethod(string dir, string input)
    {
        string rtn = input;
        int idx = 0;
        while (System.IO.File.Exists(dir + rtn))
        {
            idx += 1;
            int pos = input.LastIndexOf(".");
            if (pos == -1) rtn = input + "_" + idx.ToString();
            else rtn = input.Substring(0, pos) + "_" + idx.ToString() + input.Substring(pos);
        }
        return rtn;
    }

    private int ucUploadDocument_SuccessEvent(string fileName)
    {
        string saveName = fileName;
        int pos = fileName.LastIndexOf("\\");
        if (pos != -1) saveName = fileName.Substring(pos + 1);

        PaperRequestDocumentData data = new PaperRequestDocumentData();
        data.PaperRequestQueueID = PaperRequestQueueID;
        data.Name = ucUploadDocument.DocumentName;
        data.FileName = saveName;
        data.Description = ucUploadDocument.DocumentDescription;
        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        this.Model = data;

        ucUploadDocument.DocumentName = ucUploadDocument.DocumentDescription = string.Empty;
        return presenter.InsertPaperRequestDocument();
    }
    #endregion


}