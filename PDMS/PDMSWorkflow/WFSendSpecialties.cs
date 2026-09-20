using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFSendSpecialties : BaseWorkflowTask, IWorkflowTask
    {
        public WFSendSpecialties(int processID, int stepID)
            : base(processID, stepID)
        {
            //
            // TODO: Add constructor logic here
            //
        }
        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);
            bool returnValue = false;

            Process processID = new Process(ProcessID);
            //int partyID = 0;
            //int serviceLocationID = 0;

            try
            {
//TODO REGTOLIVE CHECK IF NEEDED
            //    if (!int.TryParse(GetProcessParameter(Constants.ProcessParameter.PartyID), out partyID))
            //    {
            //        throw new Exception(string.Format(
            //            Constants.LogString.WorkflowParameterNotNumeric,
            //            Assembly.GetExecutingAssembly().GetName().Name,
            //            Constants.ProcessParameter.PartyID,
            //            GetProcessParameter(Constants.ProcessParameter.PartyID)));
            //    }
            //    if (!int.TryParse(GetProcessParameter(Constants.ProcessParameter.ServiceLocationID), out serviceLocationID))
            //    {
            //        throw new Exception(string.Format(
            //            Constants.LogString.WorkflowParameterNotNumeric,
            //            Assembly.GetExecutingAssembly().GetName().Name,
            //            Constants.ProcessParameter.ServiceLocationID,
            //            GetProcessParameter(Constants.ProcessParameter.ServiceLocationID)));
            //    }

            //    DataSet ds = GetProviderSpecialties(partyID);

            //    if (Methods.HasRows(ds) && ds.Tables[0].Rows.Count > 1)
            //    {
            //        DataTable specialties = ds.Tables[0].Select("PrimaryFlag = 0").CopyToDataTable();

            //        //insert a TQ record for each specialty found.
            //        int idx = 0;
            //        foreach (DataRow row in specialties.Rows)
            //        {
            //            int transactionTypeID = idx == 0 ? (int)TransactionController.TransactionType.SendSpecialty2ToMMIS : (int)TransactionController.TransactionType.SendSpecialty3ToMMIS;
            //            //if have more than 2 additional specialties, will still only insert two tq records - insert is unique at the partyId and transactionTypeID
            //            int tqId = TransactionController.InsertTransactionQueue(
            //                transactionTypeID,
            //                partyID,
            //                serviceLocationID,
            //                DateTime.Now,
            //                null,
            //                null,
            //                DateTime.Now,
            //                Constants.appWorkflowUserId
            //                );

            //            SetProcessParameter(Constants.ProcessParameter.TransactionQueueID, tqId.ToString());
            //            SaveProcessParameters();
            //            idx++;
            //        }
            //    }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return returnValue;
        }
        override public string NextStep()
        {
            return null;
        }

        private DataSet GetProviderSpecialties(int partyID)
        {
            DataSet ds = null;
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PARTY_ID", DbType.Int32, partyID, true));

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectTAXONOMYByPartyID", parameters, "Specialties");

            }
            catch (Exception ex)
            {
                CoreException.ThrowException(LogThreadID, ex, logProcessName);
            }
            return ds;
        }

    }
}

