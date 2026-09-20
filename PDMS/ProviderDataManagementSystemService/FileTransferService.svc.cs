using Corp.Core.Libraries.Interface;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MAXIMUS.Services.PDMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransferService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TransferService.svc or TransferService.svc.cs at the Solution Explorer and start debugging.
    public class FileTransferService : IFileTransferService
    {

        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            RemoteFileInfo result = new RemoteFileInfo();
            try
            {
                string newFileName = CleanFilePath(request.FileName);
                newFileName = newFileName.Substring(newFileName.LastIndexOf("\\") + 1);
                string keyValue = "RegistrationApplication_Path"; // change later
                //PDF E2E
                //var configPath = "C:\\";
                var configPath = MAXIMUS.Core.Libraries.AppSettings.Get(keyValue, string.Empty);
#if DEBUG
                configPath = @"C:\Temp\OHPNM\";
#endif
                configPath = Path.Combine(configPath, "Temporary_Files");
                string filePath = System.IO.Path.Combine(configPath, newFileName);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

                // check if exists
                if (!fileInfo.Exists)
                {
                    MAXIMUS.Core.Libraries.Logging log = new Core.Libraries.Logging();
                    log.CreateLogEntry("File not found: " + filePath);
                    throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("File not found: " + filePath));
                }

                // open stream
                System.IO.FileStream stream = new System.IO.FileStream(filePath,
                          System.IO.FileMode.Open, System.IO.FileAccess.Read);

                // return result 
                result.FileName = newFileName;
                result.Length = fileInfo.Length;
                result.FileByteStream = stream;
            }
            catch (Exception ex)
            {
                MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception(ex.Message));
            }
            return result;
        }

        public string GetFileContents(string fileName)
        {
            string strFileContents = string.Empty;
            MAXIMUS.Core.Libraries.Logging log = new Core.Libraries.Logging();           
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

                if (!fileInfo.Exists)
                {
                   
                    log.CreateLogEntry("File not found: " + fileName);
                    throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("File not found: " + fileName));
                }
                using (StreamReader srFile = new StreamReader(fileName))
                {
                    strFileContents = srFile.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Exception in GetFileContents: " + ex.Message);
            }
            return strFileContents;
        }

        private string CleanFilePath(string path, bool allowRootPath = false, bool allowUNCPath = false)
        {
            var PathTrans = @"(^\.\./|(?<=/)\.\./)"; PathTrans = PathTrans.Replace(@"/", Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString())); //replace slash with platform specific separator character.
            var invalidchars = Regex.Escape(new string(System.IO.Path.GetInvalidPathChars()));
            var driveRoot = "^[a-zA-Z]:" + Regex.Escape(System.IO.Path.DirectorySeparatorChar.ToString());
            var wildcards = "*?";
            var unc = System.IO.Path.DirectorySeparatorChar.ToString() + System.IO.Path.DirectorySeparatorChar.ToString();
            //order of sanitation is vital do not change. removing in different order could create unsafe patterns.
            path = System.Text.RegularExpressions.Regex.Replace(path, "[" + invalidchars + wildcards + "]", ""); //remove invalid chars and wildcards. This must happen first 
            if (allowRootPath)
            {
                var clean = false;
                var limit = 0;
                do
                {
                    var dr = "";
                    var cxPath = path;
                    if (!allowUNCPath && cxPath.StartsWith(unc)) cxPath = cxPath.Substring(unc.Length); //remove double slash unc root
                    var drMatch = System.Text.RegularExpressions.Regex.Match(cxPath, driveRoot); //match drive segment
                    if (drMatch.Success)
                    {
                        cxPath = cxPath.Substring(drMatch.Value.Length); //get path after drive segment
                        dr = drMatch.Value;
                    }
                    cxPath = dr + cxPath.Replace(":", "");

                    if (path == cxPath) clean = true; //eliminate ':' not in drive segment
                    else path = cxPath;
                    if (limit++ == 100) throw new ApplicationException("File path contains too many invalid characters");
                } while (clean == false);
            }
            else
            {
                var clean = false;
                var limit = 0;
                do
                {
                    var cxPath = path;
                    if (!allowUNCPath && cxPath.StartsWith(unc)) cxPath = cxPath.Substring(unc.Length); //remove double slash unc root
                    if (cxPath.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString())) cxPath = cxPath.Substring(1); //remove leading slash
                    cxPath = System.Text.RegularExpressions.Regex.Replace(cxPath, driveRoot, ""); //remove DriveRoot i.e. C:\
                    cxPath = cxPath.Replace(":", "");
                    if (path == cxPath) clean = true; else path = cxPath;
                    if (limit++ == 100) throw new ApplicationException("File path contains too many invalid characters");
                } while (clean == false);
            }
            path = System.Text.RegularExpressions.Regex.Replace(path, PathTrans, ""); //remove path transversals i.e. '../' 
            return path;
        }
    }
}
