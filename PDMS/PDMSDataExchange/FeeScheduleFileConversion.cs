using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using Codaxy.WkHtmlToPdf;
using Corp.Core.Libraries;

namespace MAXIMUS.DataExchange.PDMS
{
    public class FeeScheduleFileConversion : BaseJob, IJob
    {
        private const string feeScheduleJobId = "62ED4604-3C13-4D6E-91FD-B1A4097D014A";
        private const string dbfId = "Id";
        private const string dbrLastRan = "LastRan";
        string convertHtmlFilename = string.Empty;
        string convertPdfFilename = string.Empty;
        public FeeScheduleFileConversion(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        //Debugging Mode
        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(feeScheduleJobId));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            ConvertFiles(jobGuid);
        }
        private DateTime GetJobDetail(string jobId)
        {
            DateTime lastRanTime = new DateTime();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter(dbfId, DbType.Guid, jobId, false));
                DataSet jobs = DataAccess.ExecuteStoredProcedure("usp_GetJobById", parameters, "Jobs");
                if (jobs != null && jobs.Tables.Count > 0 && jobs.Tables[0].Rows.Count > 0)
                {
                    lastRanTime = Convert.IsDBNull(jobs.Tables[0].Rows[0][dbrLastRan]) ? lastRanTime : Convert.ToDateTime(jobs.Tables[0].Rows[0][dbrLastRan]);
                    return lastRanTime;
                }
                return lastRanTime;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void ConvertFiles(string jobGuid)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            DataTable ErrorLogs = new DataTable();
            ErrorLogs.Columns.Add("Eror Message");
            ErrorLogs.Clear();
            try
            {
                DateTime lastRanTime = GetJobDetail(jobGuid);
                string feeScheduleFilePath = AppSettings.Get("FeeScheduleJobFolder");
                string feeScheduleFileInPutPath = string.Format("{0}{1}", feeScheduleFilePath, "CSV"),   
                    feeScheduleFileOutPutPath = string.Format("{0}{1}", feeScheduleFilePath, "OUTPUT"),    
                    feeScheduleFileProdPath = string.Format("{0}{1}", feeScheduleFilePath, "DISPLAY");        
                Directory.CreateDirectory(feeScheduleFileOutPutPath);
                Directory.CreateDirectory(feeScheduleFileProdPath);

                var CSVdir = new DirectoryInfo(feeScheduleFileInPutPath);
                var OUTPUTdir = new DirectoryInfo(feeScheduleFileOutPutPath);
                var DISPLAYdir = new DirectoryInfo(feeScheduleFileProdPath);

                //taking a list of files which are uncommon in CSV and OUTPUT folder and then deleting each file
                //from OUTPUT folder

                List<string> result = Directory
                           .EnumerateFiles(OUTPUTdir.ToString(), "*.*", SearchOption.AllDirectories)
                           .Select(A => Path.GetFileNameWithoutExtension(A).ToUpper())
                           .Except(Directory
                              .EnumerateFiles(CSVdir.ToString(), "*.*", SearchOption.AllDirectories)
                              .Select(B => Path.GetFileNameWithoutExtension(B).ToUpper()),
                               StringComparer.OrdinalIgnoreCase)
                           .OrderBy(name => name).ToList();

                if (result.Count > 0)
                {
                    foreach (FileInfo outputfile in OUTPUTdir.GetFiles())
                    {
                        string outputfilename = Path.GetFileNameWithoutExtension(outputfile.ToString()).ToUpper();
                        if (result.Contains(outputfilename))
                        {
                            //Removing uncommon files from OUTPUT folder
                            File.Delete(OUTPUTdir + "\\" + outputfile.ToString());
                            File.Delete(OUTPUTdir + "\\" + outputfile.Name.Replace(".CSV", ".html").ToString());
                            File.Delete(OUTPUTdir + "\\" + outputfile.Name.Replace(".CSV", ".pdf").ToString());
                        }
                    }
                }

                //Reading files from CSV folder
                List<FileInfo> files = new DirectoryInfo(feeScheduleFileInPutPath).GetFiles("*.*").OrderByDescending(f => f.LastAccessTime).ToList();

                //Taking filenamewithpath,filename,Date into a Tuple
                Tuple<string, string, string>[] FileDetails = Directory.EnumerateFiles(feeScheduleFileInPutPath, "*.*")
                                               .Select(
                                               fn => Tuple.Create(Path.GetFileName(fn),
                                               (Path.GetFileNameWithoutExtension(fn).Remove(Path.GetFileNameWithoutExtension(fn).IndexOf("Contract"))).ToUpper().Replace("_", " ").Trim(),
                                               (Path.GetFileNameWithoutExtension(fn).Substring(Path.GetFileNameWithoutExtension(fn).Length - 10).Replace("_", " ").Replace("t", "").TrimStart().Replace(" ", "/")).Trim())
                                               ).ToArray();

                //Filtering files with latest version of file from CSV Folder
                List<string> filteredfiles = (from p in FileDetails
                                        group p by p.Item2.ToString()
                                        into g
                                        select g.Select(m => m.Item1.ToUpper().ToString()).Max()).ToList();

                

                foreach (var file in files)
                {
                    if(filteredfiles.Contains(file.Name.ToUpper().ToString()))
                    { 
                    var fileName = file.Name.ToLower();
                    var fileNameWithPath = file.ToString();
                    var tuple = GetFileName(file);
                    //see if the file exists in \output path
                    if (File.Exists(feeScheduleFileOutPutPath + "\\" + file.Name)) 
                    {
                        //delete csv old file from Output folder
                        File.Delete(feeScheduleFileOutPutPath + "\\" + file.Name);
                        //delete html old file from Output folder
                        File.Delete(feeScheduleFileOutPutPath + "\\" + fileName.Replace(".csv", ".html"));
                        //delete pdf old file from Output folder
                        File.Delete(feeScheduleFileOutPutPath + "\\" + fileName.Replace(".csv", ".pdf"));

                        //Delete .csv,.html and .pdf files from Display folder
                        string[] csvFiles = Directory.GetFiles(feeScheduleFileProdPath, tuple.Item2 + "*.CSV");
                        if (csvFiles.Count() > 0)
                        {
                            foreach (var item in csvFiles)
                            {
                                File.Delete(item);
                            }
                        }
                        string[] htmlFiles = Directory.GetFiles(feeScheduleFileProdPath, tuple.Item2 + "*.html");
                        if (htmlFiles.Count() > 0)
                        {
                            foreach (var item in htmlFiles)
                            {
                                File.Delete(item);
                            }
                        }
                        string[] pdfFiles = Directory.GetFiles(feeScheduleFileProdPath, tuple.Item2 + "*.pdf");
                        if (pdfFiles.Count() > 0)
                        {
                            foreach (var item in pdfFiles)
                            {
                                File.Delete(item);
                            }
                        }
                    }

                    //upload new file to output folder and generate .html and .pdf
                    try
                    {
                        //Upload CSV to output folder.
                        copyToOutput(feeScheduleFileInPutPath, feeScheduleFileOutPutPath, fileNameWithPath);

                        //Convert CSV to HTML
                        var htmlFile = ConvertToHtml(feeScheduleFileInPutPath, feeScheduleFileOutPutPath, fileNameWithPath);

                        // Convert stringHTML to PDF
                        Guid logThreadId = Guid.NewGuid();
                        MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);
                        convertHtmlFilename = feeScheduleFileInPutPath + "\\" + Path.GetFileName(htmlFile);
                        doc.ConvertHTMLToPDF(feeScheduleFileOutPutPath, "\\" + Path.GetFileName(htmlFile), feeScheduleFileOutPutPath);

                        string revisedFilename = fileName.Replace(feeScheduleFileOutPutPath + "\\", "");
                        string sourcepath = string.Format("{0}\\{1}", feeScheduleFileOutPutPath, revisedFilename);
                        string destinationpath = string.Format("{0}\\{1}", feeScheduleFileProdPath, revisedFilename);
                        System.IO.File.Copy(sourcepath, destinationpath, true);

                        //Delete entry from fee schedule table
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("Category_Name", DbType.String, tuple.Item1, true));
                        DataAccess.ExecuteStoredProcedure("Usp_Delete_Category_Fee_Schedule", parameters);

                        //Add entry to fee schedule table
                        AddFeeSchedule(tuple.Item1, tuple.Item2);
                    }
                    catch (PdfConvertTimeoutException ex) { ErrorLogs.Rows.Add(ex.Message); }
                    catch (PdfConvertException ex) { ErrorLogs.Rows.Add(ex.Message); }
                    catch (Exception ex)
                    {
                        log.CreateLogEntry(ex.Message + "\r\n" + ex.StackTrace, Logging.LogPriority.Error);
                        ErrorLogs.Rows.Add(ex.Message);
                    }
                    }
                    else
                    {
                        //Removing files from OUTPUT folder
                        File.Delete(OUTPUTdir + "\\" + file.ToString());
                        File.Delete(OUTPUTdir + "\\" + file.Name.Replace(".CSV", ".html").ToString());
                        File.Delete(OUTPUTdir + "\\" + file.Name.Replace(".CSV", ".pdf").ToString());
                    }
                }
                if (ErrorLogs.Rows.Count == 0)
                {
                    //If no error occurs while converting file then
                    //Deleting Production Files (DISPLAY Folder)
                    System.IO.DirectoryInfo ProductionFilesDirectory = new System.IO.DirectoryInfo(feeScheduleFileProdPath);
                    foreach (FileInfo productionFile in ProductionFilesDirectory.GetFiles())
                    { productionFile.Delete(); }

                    //Copy files over to //Display folder
                    string[] filePaths = Directory.GetFiles(feeScheduleFileOutPutPath);
                    foreach (var filename in filePaths)
                    {
                        string revisedFilename = filename.Replace(feeScheduleFileOutPutPath + "\\", "");
                        string sourcepath = string.Format("{0}\\{1}", feeScheduleFileOutPutPath, revisedFilename);
                        string destinationpath = string.Format("{0}\\{1}", feeScheduleFileProdPath, revisedFilename);

                        System.IO.File.Copy(sourcepath, destinationpath, true);
                    }
                }             
            }
            catch (Exception ex)
            {
                if (convertHtmlFilename.ToString() != string.Empty)
                {
                    File.Delete(convertHtmlFilename);
                }
                log.CreateLogEntry("Failed to Convert Fee schedule files in Job"
                                     + " Exception Message " + ex.Message + " Exception Stack = "
                                     + ex.StackTrace, Logging.LogPriority.Error);
            }
            finally
            {
                string errorRowsCount = ErrorLogs.Rows.Count.ToString();
                if (ErrorLogs.Rows.Count > 0)
                {
                    string error = string.Format("{0} File conversion has failed", errorRowsCount);
                    throw new Exception(error);
                }
            }
        }

        private static Tuple<string, string> GetFileName(FileInfo file)
        {
            string csvFilename = file.Name.ToString();
            string modifieddateposition;
            string monthModifiedRefData = "";
            string referenceDateposition = csvFilename.Substring(csvFilename.Length - 15);
            string dateposition = csvFilename.Substring(csvFilename.Length - 15);
            string modifieddatedate = dateposition.Substring(dateposition.Length - 12);
            char dateChar = modifieddatedate[1];
            if ((dateChar.ToString() == "_") || (dateChar.ToString() == "-") || (dateChar.ToString() == " "))
            {
                modifieddateposition = dateposition.Insert(5, "0");
                monthModifiedRefData = referenceDateposition.Remove(0, 1);
            }
            else
            {
                modifieddateposition = dateposition;
                monthModifiedRefData = referenceDateposition;
            }

            string monthposition = modifieddateposition.Substring(modifieddateposition.Length - 14);
            char monthChar = monthposition[0];

            if ((monthChar.ToString() == "_") || (monthChar.ToString() == "-") || (monthChar.ToString() == " "))
            {
                modifieddateposition = monthposition.Insert(1, "0");
                monthModifiedRefData = monthModifiedRefData.Remove(0, 1);
            }

            string latestCsvFileName = csvFilename.Replace(monthModifiedRefData, modifieddateposition);
            string categoryName = latestCsvFileName.Remove(latestCsvFileName.Length - 24).Replace("_", " ").Replace("-", " ");
            categoryName = categoryName.Replace("Fee Schedule Procedure ", "");
            string filename = file.Name.ToString().Replace(".CSV", "");

            return new Tuple<string, string>(categoryName, filename);
        }

        private void AddFeeSchedule(string categoryName, string filename)
        {
            List<SqlParameter> parms = new List<SqlParameter>()
                {
                new SqlParameter("@Category_Name", categoryName),
                new SqlParameter("@File_Name", filename),
                new SqlParameter("@LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString()),
                };

            DataAccess.ExecuteStoredProcedure("Usp_Insert_Category_Fee_Schedule", parms);
        }

        private static string ConvertToHtml(string feeScheduleFileCSVPath, string feeScheduleFileHTMLPath, string fileNameWithPath)
        {
            try
            {

                string fileName = Path.GetFileNameWithoutExtension(fileNameWithPath);
                FileInfo file = new FileInfo(fileNameWithPath);


                
                string[] lines = System.IO.File.ReadAllLines(feeScheduleFileCSVPath + "\\" + fileNameWithPath);
                StringBuilder stringHTML = new StringBuilder();

                stringHTML.Append("<table border=1 cellpadding=2 cellspacing=0 bordercolor='black' width='100%'>");
                foreach (string line in lines)
                {
                    stringHTML.Append("<tr>");
                    var lineParse = line.Trim(',');
                    string[] columns = lineParse.Split(',');
                    foreach (string column in columns)
                    {
                        stringHTML.Append("<td>");
                        stringHTML.Append(column.Replace("\"", "").Replace("=", ""));
                        stringHTML.Append("</td>");
                    }
                    stringHTML.Append("</tr>");
                }
                stringHTML.Append("</table>");
                string htmlFile = feeScheduleFileHTMLPath + "\\" + fileName + ".html";
                File.WriteAllText(htmlFile, stringHTML.ToString());
                return htmlFile;
            }
            catch(Exception ex)
            {
                var ex2 = new Exception("Failed HTML Conversion of file: " + fileNameWithPath, ex);
                throw ex2;
                
            }
        }

        public void copyToOutput(string feeScheduleFileCSVPath, string feeScheduleFileoutputCSVPath, string fileNameWithPath)
        {
            try
            {
                string sourcepath = string.Format("{0}\\{1}", feeScheduleFileCSVPath, fileNameWithPath);
                string destinationpath = string.Format("{0}\\{1}", feeScheduleFileoutputCSVPath, fileNameWithPath);
                
                System.IO.File.Copy(sourcepath, destinationpath, true);
            }
            catch { throw; }
        }
    }
}