using MAXIMUS.Core.Libraries;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

public partial class UserControls_DataFix_DelegateAffiliationUploads : System.Web.UI.UserControl
{


    public string DestinationPath
    {
        get { return ViewState["_DestinationPath"] == null ? null : Helper.CleanFilePath(ViewState["_DestinationPath"].ToString(), true); }
        set
        {
            ViewState["_DestinationPath"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(out totalResultCount, gvDelegateAffiliationUploadsResults.PageSize, startRowIndex); 
        if (totalResultCount == 0)
        {
            gvDelegateAffiliationUploadsResults.EmptyDataText = "No Transactions found.";
            gvDelegateAffiliationUploadsResults.DataSource = null;
            gvDelegateAffiliationUploadsResults.DataBind();
        }
        gvDelegateAffiliationUploadsResults.DataSource = dt;
        gvDelegateAffiliationUploadsResults.VirtualItemCount = totalResultCount;
        gvDelegateAffiliationUploadsResults.DataBind();
    }

    protected void gvDelegateAffiliationUploadsResults_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvDelegateAffiliationUploadsResults.PageIndex = e.NewPageIndex;
        gvDelegateAffiliationUploadsResults.VirtualItemCount = 1000;
        gvDelegateAffiliationUploadsResults.DataBind();
    }

    protected void gvDelegateAffiliationUploadsResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = 0;
        if (e.CommandName == "upload")
        {
            index = Convert.ToInt32(e.CommandArgument);
            bool fileDownloaded = false;
            string filename = this.gvDelegateAffiliationUploadsResults.DataKeys[index].Values["DELEGATE_FILE_NAME"].ToString();
            DownloadFile(filename, out fileDownloaded);
        }
        else if (e.CommandName == "response")
        {
            index = Convert.ToInt32(e.CommandArgument);
            bool fileDownloaded = false;
            string filename = this.gvDelegateAffiliationUploadsResults.DataKeys[index].Values["RESPONSE_DELEGATE_FILE_NAME"].ToString();
            DownloadFile(filename, out fileDownloaded);
        }
    }


    private void DownloadFile(string fileName, out bool fileDownloaded)
    {
        fileDownloaded = false;
        try
        {
            string filePath = string.Empty;
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);

            // If isFileLocal is false, that means file is on onbase. 
            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {
                AddError("File or directory does not exist.: " + filePath, "DataFixDelegates");
            }
            else
            {
                fileDownloaded = true;
            }

            bool fileExistsonLocal = System.IO.File.Exists(filePath);
            bool downloadFile = isFileLocal ? fileExistsonLocal : true;

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
            AddError(ex.Message + " " + ex.StackTrace.ToString(), "DataFixDelegates");
        }

    }

    private void AddError(string errMsg, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
    }

    protected void gvDelegateAffiliationUploadsResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int totalResultCount = 0;
        int startRowIndex = gvDelegateAffiliationUploadsResults.PageSize * e.NewPageIndex;
        DataTable dt = GetData(out totalResultCount, gvDelegateAffiliationUploadsResults.PageSize, startRowIndex);
        if (totalResultCount == 0)
        {
            gvDelegateAffiliationUploadsResults.EmptyDataText = "No Transactions found.";
            gvDelegateAffiliationUploadsResults.DataSource = null;
            gvDelegateAffiliationUploadsResults.DataBind();
        }
        gvDelegateAffiliationUploadsResults.PageIndex = e.NewPageIndex;
        gvDelegateAffiliationUploadsResults.DataSource = dt;
        gvDelegateAffiliationUploadsResults.VirtualItemCount = totalResultCount;
        gvDelegateAffiliationUploadsResults.DataBind();
    }

    private DataTable GetData(out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = PassthroughController.SelectDelegateAffiliationUploadResults(pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDelegateAffiliationFileID.Text))
            {
                int response = 0;
                DataSet returnValue = new DataSet();
                if (chkReProcessOnlyStagingID.Checked == true)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, txtDelegateAffiliationFileID.Text, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.appAdminUserId, false));
                    parameters.Add(SqlParms.CreateParameter("SETOPERATION", DbType.Int32, CON.DelegateAffilationReprocessing.ReProcesOnlyStaging, false));
                    DataAccess.ExecuteStoredProcedure("usp_ReProcess_Delegate_Affiliate_File", parameters);
                    response = 1;
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, txtDelegateAffiliationFileID.Text, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.appAdminUserId, false));
                    parameters.Add(SqlParms.CreateParameter("SETOPERATION", DbType.Int32, CON.DelegateAffilationReprocessing.Default, false));
                    returnValue = DataAccess.ExecuteStoredProcedure("usp_ReProcess_Delegate_Affiliate_File", parameters, "ReProcess_Delegate_Affiliate");
                    DataTable dt = new DataTable();
                    dt = returnValue.Tables[0];
                    DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;

                    if (int.TryParse(Methods.GetString("OUTPUT", dr), out response))
                    {

                    }
                }
                if (response == 0)
                {
                    lblButtonResponse.Text = "Record ID : " + txtDelegateAffiliationFileID.Text + " doesn't Exists!";
                }
                else
                {
                    lblButtonResponse.Text = "Record ID : " + txtDelegateAffiliationFileID.Text + " updated successfully!";
                    int totalResultCount = 0;
                    int startRowIndex = 0;
                    DataTable dtt = GetData(out totalResultCount, gvDelegateAffiliationUploadsResults.PageSize, startRowIndex);
                    if (totalResultCount == 0)
                    {
                        gvDelegateAffiliationUploadsResults.EmptyDataText = "No Transactions found.";
                        gvDelegateAffiliationUploadsResults.DataSource = null;
                        gvDelegateAffiliationUploadsResults.DataBind();
                    }
                    gvDelegateAffiliationUploadsResults.DataSource = dtt;
                    gvDelegateAffiliationUploadsResults.VirtualItemCount = totalResultCount;
                    gvDelegateAffiliationUploadsResults.DataBind();
                 }
            }
            else
            {
                AddError("Document ID required.", "DataFixDelegates");
            }
        }
        catch(Exception ex)
        {
            lblButtonResponse.Text = "Error while Reprocessing the Record " + txtDelegateAffiliationFileID.Text + " " + ex.Message.ToString() + " " + ex.StackTrace.ToString();
        }
    }

    private void Refresh()
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dtt = GetData(out totalResultCount, gvDelegateAffiliationUploadsResults.PageSize, startRowIndex);
        if (totalResultCount == 0)
        {
            gvDelegateAffiliationUploadsResults.EmptyDataText = "No Transactions found.";
            gvDelegateAffiliationUploadsResults.DataSource = null;
            gvDelegateAffiliationUploadsResults.DataBind();
        }
        gvDelegateAffiliationUploadsResults.DataSource = dtt;
        gvDelegateAffiliationUploadsResults.VirtualItemCount = totalResultCount;
        gvDelegateAffiliationUploadsResults.DataBind();
    }


    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrWhiteSpace(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    protected void btnUpdateResponseFile_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDelegateAffiliationFileID.Text))
            {
                if (string.IsNullOrEmpty(efuUpdateResponseFileID.PostedFile.FileName))
                {
                    AddError("Document is required.", "DataFixDelegates");
                }
                else if (!IsSpecialCharacter(efuUpdateResponseFileID.FileName))
                {
                    AddError("The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): Please remove the Special Character from the file Name before upload. ", "DataFixDelegates");
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        DestinationPath = @"C:\Temp\";
                    }
                    else
                    {
                        DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
                    }

                    if (!Directory.Exists(DestinationPath))
                    {
                        Directory.CreateDirectory(DestinationPath);
                    }
                    string fileName = Helper.CleanFilePath(efuUpdateResponseFileID.FileName);
                    byte[] fileBytes = efuUpdateResponseFileID.EncryptedFileBytes;

                    string newFileName = RenameFileMethod(DestinationPath, fileName);
                    File.WriteAllBytes(Path.Combine(@DestinationPath, newFileName), fileBytes);

                    string fileDescp = string.Empty;

                    int docID = SaveUploadedFile(fileName, newFileName, fileDescp);
                    if (docID != 0)
                    {

                        SendToCMS(docID, fileBytes, newFileName);

                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, txtDelegateAffiliationFileID.Text, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.appAdminUserId, false));
                        parameters.Add(SqlParms.CreateParameter("SETOPERATION", DbType.Int32, CON.DelegateAffilationReprocessing.UpdateResponseDocID, false));
                        parameters.Add(SqlParms.CreateParameter("DOCID", DbType.Int32, docID, false));
                        DataAccess.ExecuteStoredProcedure("usp_ReProcess_Delegate_Affiliate_File", parameters);

                        lblButtonResponse.Text = "Record ID : " + txtDelegateAffiliationFileID.Text + " updated successfully!";
                        Refresh();
                    }
                }
            }
            else
            {
                AddError("Document ID required.", "DataFixDelegates");
            }
        }
        catch (Exception ex)
        {
            lblButtonResponse.Text = "Error while Reprocessing the Record " + txtDelegateAffiliationFileID.Text + " " + ex.Message.ToString() + " " + ex.StackTrace.ToString();
        }
    }

    private string RenameFileMethod(string dir, string input)
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

    protected void btnUpdateUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtDelegateAffiliationFileID.Text))
            {
                if (string.IsNullOrEmpty(efuUpdateUploadFileID.PostedFile.FileName))
                {
                    AddError("Document is required.", "DataFixDelegates");
                }
                else if (!IsSpecialCharacter(efuUpdateUploadFileID.FileName))
                {
                    AddError("The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): Please remove the Special Character from the file Name before upload. ", "DataFixDelegates");
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        DestinationPath = @"C:\Temp\";
                    }
                    else
                    {
                        DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
                    }

                    if (!Directory.Exists(DestinationPath))
                    {
                        Directory.CreateDirectory(DestinationPath);
                    }
                    string fileName = Helper.CleanFilePath(efuUpdateUploadFileID.FileName);
                    byte[] fileBytes = efuUpdateUploadFileID.EncryptedFileBytes;

                    string newFileName = RenameFileMethod(DestinationPath, fileName);
                    File.WriteAllBytes(Path.Combine(@DestinationPath, newFileName), fileBytes);

                    string fileDescp = string.Empty;

                    int docID = SaveUploadedFile(fileName, newFileName, fileDescp);
                    if (docID != 0)
                    {

                        SendToCMS(docID, fileBytes, newFileName);

                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, txtDelegateAffiliationFileID.Text, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, CON.appAdminUserId, false));
                        parameters.Add(SqlParms.CreateParameter("SETOPERATION", DbType.Int32, CON.DelegateAffilationReprocessing.UpdateUploadDocID, false));
                        parameters.Add(SqlParms.CreateParameter("DOCID", DbType.Int32, docID, false));
                        DataAccess.ExecuteStoredProcedure("usp_ReProcess_Delegate_Affiliate_File", parameters);

                        lblButtonResponse.Text = "Record ID : " + txtDelegateAffiliationFileID.Text + " updated successfully!";
                        Refresh();
                    }
                }
            }
            else
            {
                AddError("Document ID required.", "DataFixDelegates");
            }

        }
        catch (Exception ex)
        {
            lblButtonResponse.Text = "Error while Reprocessing the Record " + txtDelegateAffiliationFileID.Text + " " + ex.Message.ToString() + " " + ex.StackTrace.ToString();
        }
    }



    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(docID, fileBytes, fileName);
    }

    private int SaveUploadedFile(string fileName, string newFileName, string fileDescription)
    {
        int docID = 0;
        try
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Name", DbType.String, fileName, true));
            parameters.Add(SqlParms.CreateParameter("Description", DbType.String, fileDescription, true));
            parameters.Add(SqlParms.CreateParameter("File_Name", DbType.String, fileName, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, CON.appAdminUserId, true));
            docID = Convert.ToInt32(DataAccess.ExecuteScalar("insertUPLOAD_DOCUMENT", parameters));
            return docID;
        }
        catch (Exception ex)
        {
            AddError(ex.Message + " " + ex.StackTrace.ToString(), "DataFixDelegates");
            return docID;
        }

    }

    protected void txtDelegateAffiliationFileID_TextChanged(object sender, EventArgs e)
    {
        lblButtonResponse.Text = "";
    }
}