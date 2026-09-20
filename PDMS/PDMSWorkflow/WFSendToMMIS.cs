using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

// TODO: Step 9 from New Registration Visio
namespace PDMSWorkflow
{
    public class WFSendToMMIS : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;
        public WFSendToMMIS(int processID, int stepID)
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

            Process p = new Process(ProcessID);
            int regId = 0;
            int sId = 0;
            string mId = string.Empty;
            bool successfulTransaction = true;


            try
            {
                //TODO REGTOLIVE ******************************/

                if (!int.TryParse(GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regId))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }
                //if (!int.TryParse(GetProcessParameter(Constants.ProcessParameter.ServiceLocationID), out sId))
                //{
                //    throw new Exception(string.Format(
                //        Constants.LogString.WorkflowParameterNotNumeric,
                //        Assembly.GetExecutingAssembly().GetName().Name,
                //        Constants.ProcessParameter.ServiceLocationID,
                //        GetProcessParameter(Constants.ProcessParameter.ServiceLocationID)));
                //}

                //int programStatus;
                //if (!int.TryParse(GetProcessParameter(Constants.ProcessParameter.RegProgramStatusTypeID), out programStatus))
                //{
                //    throw new Exception(string.Format(
                //        Constants.LogString.WorkflowParameterNotNumeric,
                //        Assembly.GetExecutingAssembly().GetName().Name,
                //        Constants.ProcessParameter.RegProgramStatusTypeID,
                //        GetProcessParameter(Constants.ProcessParameter.RegProgramStatusTypeID)));
                //}


                //TODo Check for successful transaction flags to set the successfulTransaction value.



                if (p.WorkflowID==CON.WorkflowType.RiskAlertClosure)
                {
                    nextStep = "Send Letter";

                    //Check for Effective date to consider it as approved
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                    DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", sqlParms, "regClosureEffective");

                    if (successfulTransaction)
                    {
                        if (ObjectControllerHelper.HasRows(dsReg))
                        {
                            DataRow drReg = dsReg.Tables[0].Rows[0];
                            string effectiveDate = ObjectControllerHelper.GetString("Closure_Effective_date", drReg);
                            if (!string.IsNullOrEmpty(effectiveDate))
                            {
                                nextStep = "Next";
                            }
                           
                        }
                    }
                    else
                    {
                        nextStep = "Transaction Monitor";
                    }

                    returnValue = true;
                    return returnValue;
                }
                if (p.WorkflowID == CON.WorkflowType.RiskAlertCHOP)
                {

                    //Check for Final date of CHOP to consider it as approved
                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                    sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                    DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREG_LTC_RISK_ALERT", sqlParms, "regCHOPEffective");

                    if (successfulTransaction)
                    {
                        nextStep = "Next"; //incase of Transaction Success by default

                        if (ObjectControllerHelper.HasRows(dsReg))
                        {
                            DataRow drReg = dsReg.Tables[0].Rows[0];
                            string finalDateCHOP = ObjectControllerHelper.GetString("CHOP_Final_Date", drReg);
                            bool chopWithDrawn = ObjectControllerHelper.GetBool("CHOP_Withdrawn", drReg);

                            //First check for withdrawn indicator.
                            if (chopWithDrawn)
                            {
                                nextStep = "Remove Risk";
                            }
                            else
                            {

                                if (!string.IsNullOrEmpty(finalDateCHOP))
                                {
                                    nextStep = "Approve";
                                }
                            }
                        }

                    }
                    else
                    {
                        nextStep = "Transaction Monitor";
                    }

                    returnValue = true;
                    return returnValue;
                }

                if (p.WorkflowID == CON.WorkflowType.CHOP)
                {
                    nextStep = "Transaction Passed";
                    returnValue = true;
                    return returnValue;
                }

                int transactionType = 0;

               // 09/16/2015:  Modified to always send an update if medicaid id exists.  Removed varying medicaid id based on transaction type..
                DataSet ds1 = SelectMedicaidId(regId);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                    sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                }
                //if (mId.Length > 0)
                if(!string.IsNullOrEmpty(mId))
                {
                    transactionType = (int)TransactionController.TransactionType.SendProviderUpdatestoMMIS;
                }
                else
                {
                    transactionType = (int)TransactionController.TransactionType.RequestMedicaidIDfromMMIS;
                }

                if (transactionType > 0)
                {
                    int tqId = TransactionController.InsertTransactionQueue(
                        transactionType,
                        regId,
                        sId,
                        DateTime.Now,
                        null,
                        null,
                        DateTime.Now,
                        Constants.appWorkflowUserId);
                    returnValue = true;

                    SetProcessParameter(Constants.ProcessParameter.TransactionQueueID, tqId.ToString());
                    SaveProcessParameters();

                    // Insert the MMIS Staging Tables
                    //PopulateStagingData(regId,tqId,transactionType); 
                }

                //bool npiUpdate = this.IsNPIUpdateNeeded(pId);

                //transactionType = (int)TransactionController.TransactionType.SendNPIUpdateToMMIS;
                //if (npiUpdate)
                //{
                //    int tqId = TransactionController.InsertTransactionQueue(
                //     transactionType,
                //     pId,
                //     sId,
                //     DateTime.Now,
                //     null,
                //     null,
                //     DateTime.Now,
                //     Constants.appWorkflowUserId);
                //    returnValue = true;
                //}

                //// Insert transaction for CBSA updates
                //bool cbsaUpdate = this.IsCBSAUpdateNeeded(pId);
                //int cbsaTransactionType = (int)TransactionController.TransactionType.SendCBSAUpdateToMMIS;

                //if (cbsaUpdate)
                //{
                //    int tqId = TransactionController.InsertTransactionQueue(
                //     cbsaTransactionType,
                //     pId,
                //     sId,
                //     DateTime.Now,
                //     null,
                //     null,
                //     DateTime.Now,
                //     Constants.appWorkflowUserId);
                //    returnValue = true;
                //}



                //REGTOLIVE fake Medicaid ID
                /*
                string mId1 = DateTime.Now.ToString("ddmyyssff");
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, true));

                parameters.Add(SqlParms.CreateParameter("Medicaid_ID", DbType.String, mId1, true));

                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));

                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, Constants.appWorkflowUserId, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("sp_UpdateMedicaidIDByRegID", parameters, "updateMedicaidID");
                */
                nextStep = null;
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
            return nextStep;
        }
        private bool IsNPIUpdateNeeded(int partyId)
        {
            bool npiUpdateNeeded = false;
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_NPIUpdateNeeded", parameters, "NPIUpdateRecords");

                npiUpdateNeeded = ds.Tables[0].Rows.Count > 0;
            }
            catch (Exception ex)
            {
                CoreException.ThrowException(LogThreadID, ex, logProcessName);
            }


            return npiUpdateNeeded;
        }

        private bool IsCBSAUpdateNeeded(int partyId)
        {
            bool cbsaUpdateNeeded = false;
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PartyID", DbType.Int32, partyId, true));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_CBSAUpdateNeeded", parameters, "cbsaUpdateRecords");

                cbsaUpdateNeeded = ds.Tables[0].Rows.Count > 0;
            }
            catch (Exception ex)
            {
                CoreException.ThrowException(LogThreadID, ex, logProcessName);
            }


            return cbsaUpdateNeeded;
        }

        private DataSet SelectMedicaidId(int regID)
        {
            //string returnVal = string.Empty;
            DataSet ds = null;
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                //****************fake Medicaid ID
               // returnVal = DateTime.Now.ToString("ddmyyssff");


                //TODO REGTOLIVE*********************************/
                //// generated by sp_Admin_StoredProcBuilder on Nov  8 2013 11:35AM
                //// create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectServiceLocationID_ByRegID", parameters, "ServiceLocations");
                ds.Tables[0].TableName = "ServiceLocations";
                //// loop over records in dataset
                //foreach (DataRow location in ds.Tables[0].Rows)
                //{
                //    returnVal = ObjectControllerHelper.GetString("MedicaidID", location);
                //}                      
            }
            catch (Exception ex)
            {
                CoreException.ThrowException(LogThreadID, ex, logProcessName);
            }
            return ds;
        }

        private void PopulateStagingData(int regID, int transactionID,int transactionType)
        {
            string logProcessName = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name
                , MethodBase.GetCurrentMethod().Name);

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("TRANSACTION_QUEUE_ID", DbType.Int32, transactionID, true));
                parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, transactionType, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appWorkflowUserId, true));

                DataAccess.ExecuteStoredProcedure("usp_InsertMMISStagingTables", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(LogThreadID, ex, logProcessName);
            }
        }
    }
}