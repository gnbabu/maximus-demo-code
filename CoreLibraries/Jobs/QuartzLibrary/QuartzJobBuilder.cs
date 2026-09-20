using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MAXIMUS.Core.Libraries.QuartzLibrary
{
    /// <summary>
    ///     Author(s):      Ahackl
    ///     Date:           2017.06.12
    ///     Name:           QuartzJob
    ///     Description:    Builds Quartz Objects from Legacy Data
    /// </summary>
    public static class QuartzJobBuilder
    {
        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Instantiates a new Quartz Job based upon job type id
        /// </summary>
        public static QuartzJob GetQuartzJob(int JobTypesID)
        {
            QuartzJob job = (
                typeof(QuartzJob).Assembly.GetTypes()
               .Where(t => t.IsSubclassOf(typeof(QuartzJob)) && !t.IsAbstract)
               .Select(t => (QuartzJob)Activator.CreateInstance(t))
               )
               .First(x => x.JobTypesID == JobTypesID);

            return job;
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Creates job data from previous job data table
        /// </summary>
        public static JobDataMap BuildJobDataFromDataTable(DataRow jobDataRow, QuartzJob job)
        {

            const string dbtJobs = "Jobs";
            const string dbtJobParams = "JobParams";
            const string dbfSendEmail = "SendEmail";
            const string dbfDllPath = "DllPath";
            const string dbfAssemblyName = "AssemblyName";
            const string dbfClassName = "ClassName";
            const string dbfEmailOnErrors = "EmailOnErrors";
            const string dbfParamValue = "JobParamsAppSettingsValue";
            const string dbfParamOrder = "JobParamsOrder";

            const string dbrJob2Parms = dbtJobs + "2" + dbtJobParams;

            const string jtInterface = "1";
            const string jtSetsFTPWithKey = "2";
            const string jtRunSQL = "3";
            const string jtRunProc = "4";
            const string jtGetProc = "5";
            const string jtSetsFTP = "6";

            string exText = string.Empty;
            string jobName = string.Empty;


            JobDataMap jobData = new JobDataMap();
            jobData.Add("JobID", jobDataRow["Id"].ToString());
            jobData.Add("EmailErrors", Methods.GetBoolean(jobDataRow[dbfEmailOnErrors]).ToString());
            jobData.Add("EmailEndOfExecution", Methods.GetBoolean(jobDataRow[dbfSendEmail]).ToString());

            // get the job type from the first row
            string filePath = string.Empty;
            string fullName = string.Empty;
            string hostName = string.Empty;
            string remotePath = string.Empty;
            string userName = string.Empty;
            string password = string.Empty;
            string hostKey = string.Empty;
            string portNumberAppSettingsKey = string.Empty;
            string privateKey = string.Empty;
            string fileName = string.Empty;
            string jobTypeId = job.JobTypesID.ToString();
            string procName = string.Empty;
            string procResultsText = string.Empty;
            string recipients = string.Empty;
            

            switch (jobTypeId.Trim())
            {
                case jtGetProc:

                    procName = string.Empty;
                    recipients = string.Empty;
                    fileName = string.Empty;
                    string wildcard = string.Empty;
                    string delimiter = "|";
                    filePath = string.Empty;

                    // if the job has parameters
                    foreach (DataRow param in jobDataRow.Table.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                    {

                        // set the value to a local string
                        string order = Convert.ToString(param[dbfParamOrder]);
                        string val = Convert.ToString(param[dbfParamValue]);

                        // depending on the order number, set value 
                        switch (order)
                        {

                            case "1":
                                procName = val;
                                break;

                            case "2":
                                filePath = val;
                                break;

                            case "3":
                                fileName = val;
                                break;

                            case "4":
                                wildcard = val;
                                break;

                            case "5":
                                delimiter = val;
                                break;
                        }
                    }

                    jobData.Add("ProcedureName", procName);
                    jobData.Add("FileNameAppSettingsKey", fileName);
                    jobData.Add("WildCardAppSettingsKey", wildcard);
                    jobData.Add("Delimiter", delimiter);
                    jobData.Add("FilePathAppSettingsKey", filePath);

                    break;

                case jtRunProc:

                    procName = string.Empty;
                    recipients = string.Empty;

                    // if the job has parameters
                    foreach (DataRow param in jobDataRow.Table.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                    {

                        // set the value to a local string
                        string order = Convert.ToString(param[dbfParamOrder]);
                        string val = Convert.ToString(param[dbfParamValue]);

                        // depending on the order number, set value 
                        switch (order)
                        {

                            case "1":
                                procName = val;
                                break;

                            case "2":
                                recipients = val;
                                break;
                        }
                    }

                    jobData.Add("ProcedureName", procName);
                    jobData.Add("RecipientsAppSettingsKey", recipients);

                    break;

                case jtRunSQL:

                    // 404CE0E4-7A6B-4A9F-B019-843847028198 - send a count of current attestation states

                    string sql = string.Empty;
                    recipients = string.Empty;

                    // if the job has parameters
                    foreach (DataRow param in jobDataRow.Table.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                    {

                        // set the value to a local string
                        string order = Convert.ToString(param[dbfParamOrder]);
                        string val = Convert.ToString(param[dbfParamValue]);

                        // depending on the order number, set value 
                        switch (order)
                        {

                            case "1":
                                sql = val;
                                break;

                            case "2":
                                recipients = val;
                                break;
                        }
                    }

                    jobData.Add("SQL", sql);
                    jobData.Add("RecipientsAppSettingsKey", recipients);

                    break;

                case jtInterface:
 
                        string assemblyName = Convert.ToString(jobDataRow[dbfAssemblyName]);
                        string className = Convert.ToString(jobDataRow[dbfClassName]);
                        string assemblyPath = Methods.GetStringValue(jobDataRow[dbfDllPath]);

                        jobData.Add("AssemblyName", assemblyName);
                        jobData.Add("ClassName", className);
                        jobData.Add("AssemblyPath", assemblyPath);

                        break;

                case jtSetsFTPWithKey:

                    filePath = string.Empty;
                    fileName = string.Empty;
                    hostName = string.Empty;
                    remotePath = string.Empty;
                    userName = string.Empty;
                    password = string.Empty;
                    hostKey = string.Empty;
                    portNumberAppSettingsKey = string.Empty;
                    privateKey = string.Empty;

                    // if the job has parameters
                    foreach (DataRow param in jobDataRow.Table.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                    {

                        // set the value to a local string
                        string order = Convert.ToString(param[dbfParamOrder]);
                        string val = Convert.ToString(param[dbfParamValue]);

                        // depending on the order number, set value 
                        switch (order)
                        {

                            case "1":
                                filePath = val;
                                break;

                            case "2":
                                fileName = val;
                                break;

                            case "3":
                                hostName = val;
                                break;

                            case "4":
                                remotePath = val;
                                break;

                            case "5":
                                userName = val;
                                break;

                            case "6":
                                password = val;
                                break;

                            case "7":
                                hostKey = val;
                                break;

                            case "8":
                                portNumberAppSettingsKey = val;
                                break;

                            case "9":
                                privateKey = val;
                                break;

                        }
                    }

                    jobData.Add("FilePathAppSettingsKey", filePath);
                    jobData.Add("FileNameAppSettingsKey", fileName);
                    jobData.Add("HostNameAppSettingsKey", hostName);
                    jobData.Add("RemotePathAppSettingsKey", remotePath);
                    jobData.Add("UserNameAppSettingsKey", userName);
                    jobData.Add("PasswordAppSettingsKey", password);
                    jobData.Add("HostKeyAppSettingsKey", hostKey);
                    jobData.Add("PortNumberAppSettingsKey", portNumberAppSettingsKey);
                    jobData.Add("PrivateKeyAppSettingsKey", privateKey); 

                    break;

                case jtSetsFTP:

                    // Credentialing File - 38FB6B9B-0965-4146-9693-DE332AACEF03

                    // set variables for call to method
                    filePath = string.Empty;
                    fileName = string.Empty;
                    hostName = string.Empty;
                    remotePath = string.Empty;
                    userName = string.Empty;
                    password = string.Empty;
                    wildcard = string.Empty;
                    hostKey = string.Empty;

                    // if the job has parameters
                    foreach (DataRow param in jobDataRow.Table.ChildRelations[dbrJob2Parms].ChildTable.Rows)
                    {

                        // set the value to a local string
                        string order = Convert.ToString(param[dbfParamOrder]);
                        string val = Convert.ToString(param[dbfParamValue]);

                        // depending on the order number, set value 
                        switch (order)
                        {

                            case "1":
                                filePath = val;
                                break;

                            case "2":
                                fileName = val;
                                break;

                            case "3":
                                wildcard = val;
                                break;

                            case "4":
                                hostName = val;
                                break;

                            case "5":
                                remotePath = val;
                                break;

                            case "6":
                                userName = val;
                                break;

                            case "7":
                                password = val;
                                break;

                            case "8":
                                hostKey = val;
                                break;
                        }
                    }

                    jobData.Add("FilePathAppSettingsKey", filePath);
                    jobData.Add("FileNameAppSettingsKey", fileName);
                    jobData.Add("HostNameAppSettingsKey", hostName);
                    jobData.Add("RemotePathAppSettingsKey", remotePath);
                    jobData.Add("UserNameAppSettingsKey", userName);
                    jobData.Add("PasswordAppSettingsKey", password);
                    jobData.Add("WildCardAppSettingsKey", wildcard);
                    jobData.Add("HostKeyAppSettingsKey", hostKey);
                   

                    break;
            }
            return jobData; 
        }


    }
}
