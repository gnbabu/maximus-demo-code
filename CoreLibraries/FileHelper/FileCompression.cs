using Ionic.Zip;
using System;
using System.Reflection;

namespace MAXIMUS.Core.Libraries
{
    public class FileCompression
    {

#region "Constructors"

        public FileCompression()
        {
            // generate a new thread id GUID
            this.ThreadId = Guid.NewGuid();
        }

        public FileCompression(Guid threadId)
        {
            this.ThreadId = threadId;
        }

#endregion
#region "Logging Objects"

        private int logCnt = 0;
        private Guid m_threadId;
        private Guid ThreadId
        {
            get
            {
                return this.m_threadId;
            }
            set
            {
                this.m_threadId = value;
            }
        }

 #endregion

        /// <summary>
        ///     Extracts all contents from a zip file
        ///     NOTE: This method will overwrite if a copy already exists out the outputDirectory location
        /// </summary>
        /// <param name="fileName">The name of the file to use for the extract.</param>
        /// <param name="outputDirectory">The location where extracted contents will be copied.</param>
        public void UnzipFile(string fileName, string outputDirectory)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                string msg = String.Format("Unzipping file [{0}] to output location [{1}]", fileName, outputDirectory);
                log.CreateLogEntry(msg, +logCnt);

                //  create a new zip object and unzip file
                ZipFile zip = new ZipFile(fileName);
                zip.ExtractAll(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
                zip.Dispose();

                msg = String.Format("File processing complete [{0}] to output location [{1}]", fileName, outputDirectory);
                log.CreateLogEntry(msg, +logCnt);
            }
            catch (Exception ex)
            {
                string fullMessage = "File action failed. File Name: {0} Reason: {1}";
                log.CreateLogEntry(String.Format(fullMessage, fileName, ex.Message), Logging.LogPriority.Error, +logCnt);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
        
        /// <summary>
        ///     Creates a zip file from a directory
        /// </summary>
        /// <param name="directoryName">The name of the directory to zip</param>
        /// <param name="outputFile">The name of the file to create from the zip</param>
        public void ZipDirectory(string directoryName, string outputFile)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                string msg = String.Format("Zipping directory [{0}] to output file [{1}]", directoryName, outputFile);
                log.CreateLogEntry(msg, +logCnt);

                //  create a new zip object
                ZipFile zip = new ZipFile();

                // zip the directory and save
                zip.AddDirectory(directoryName);
                zip.Save(outputFile);
                zip.Dispose();

                msg = String.Format("Directory processing complete [{0}] to output file [{1}]", directoryName, outputFile);
                log.CreateLogEntry(msg, +logCnt);
            }
            catch (Exception ex)
            {
                string fullMessage = "Directory action failed. Directory Name: {0} Reason: {1}";
                log.CreateLogEntry(String.Format(fullMessage, directoryName, ex.Message), Logging.LogPriority.Error, +logCnt);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        /// <summary>
        ///     Creates a zip file from a non-zipped file
        /// </summary>
        /// <param name="directoryName">The name of the directory to zip</param>
        /// <param name="outputFile">The name of the file to create from the zip</param>
        public void ZipFile(string fileName, string outputFile)
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);

            try
            {
                string msg = String.Format("Zipping file [{0}] to output file [{1}]", fileName, outputFile);
                log.CreateLogEntry(msg, +logCnt);

                //  create a new zip object
                ZipFile zip = new ZipFile();

                // zip the file and save
                zip.AddFile(fileName);
                zip.Save(outputFile);
                zip.Dispose();

                msg = String.Format("File processing complete [{0}] to output file [{1}]", fileName, outputFile);
                log.CreateLogEntry(msg, +logCnt);
            }
            catch (Exception ex)
            {
                string fullMessage = "File action failed. Directory Name: {0} Reason: {1}";
                log.CreateLogEntry(String.Format(fullMessage, fileName, ex.Message), Logging.LogPriority.Error, +logCnt);
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
    }
}
