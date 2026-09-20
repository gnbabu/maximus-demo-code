using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using Workflow;
using static MAXIMUS.Core.Libraries.Constants;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFSendPayloadForToSIDisenrollDetour : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFSendPayloadForToSIDisenrollDetour(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            int sId = 0;
            string mId = string.Empty;
            string txnResult = string.Empty;
            Process p = new Process(ProcessID);
            //bool txnSuccess = true;
            try
            {
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }
				
				// go get related (old) provider
				List<SqlParameter> sqlOldRegParms = new List<SqlParameter>();
				sqlOldRegParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlOldRegParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, false));
                DataSet dsOldReg = DataAccess.ExecuteStoredProcedure("usp_selectProviderTypeChangeRequestByNewReg", sqlOldRegParms, "OldRegData");
				dsOldReg.Tables[0].TableName = "OldRegData";
				DataRow drOldReg = ObjectControllerHelper.HasRows(dsOldReg) ? dsOldReg.Tables[0].Rows[0] : null;
				int oldRegId = ObjectControllerHelper.GetInt("OLD_REG_ID", drOldReg);
				regID = oldRegId;

                // we need to go look at REG_NPI_MEDID_ENROLLMENT_SPAN staging table to see if the end date is different than today
                DateTime terminationDate = DateTime.Now;

                DateTime enrollEndDateTime;
                List<SqlParameter> sqlStgRegEnrollParms = new List<SqlParameter>();
                sqlStgRegEnrollParms = new List<SqlParameter>();
                sqlStgRegEnrollParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                sqlStgRegEnrollParms.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, p.ProcessID, false));
                DataSet dsStgRegEnroll = DataAccess.ExecuteStoredProcedure("usp_CheckRegNPIEnrollmentExists", sqlStgRegEnrollParms, "StgRegEnrollData");
                if (ObjectControllerHelper.HasRows(dsStgRegEnroll))
                {
                    DataRow drStgRegEnroll = dsStgRegEnroll.Tables[0].Rows[0];
                    enrollEndDateTime = ObjectControllerHelper.GetDateTime("ENROLL_END_DATE_TIME", drStgRegEnroll); // this won't matter for no gap scenario; it'll actually be grabbing the inactive row's start date, but it's never used

                    if (!ObjectControllerHelper.IsDateNull(enrollEndDateTime))
                    {
                        // use the termination date from the staging table since the user entered that recently
                        terminationDate = enrollEndDateTime;
                    }
                }

                // terminate the old provider - using date.now unless we have a termination date entered into the staging table on the NPIandMedId page
                ProviderController.TerminateProvider(regID, terminationDate, CON.appAdminUserId, CON.EnrollmentStatusTypeID.InActive.ToString(), CON.EnrollStatusReason.ProvChangedProvNumbers, false, true);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ProviderManagementReqRes prr = new ProviderManagementReqRes();
                  
				string makeRequest = AppSettings.Get("MakeWSRequestCallToSI");

                if (makeRequest.ToLower().Equals("true"))
				{
					int transactionType = 0;
					int tId = 0;

					DataSet ds1 = RegistrationController.SelectSvcLocIDTranType(regID);
					if (ds1.Tables[0].Rows.Count > 0)
					{
						mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
						sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
						tId = Convert.ToInt32(ds1.Tables[0].Rows[0]["TRANSACTION_TYPE_ID"]);
					}

					transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;

					if (transactionType > 0)
					{
					   int tqId = TransactionController.InsertTransactionQueue(
							transactionType,
							regID,
							sId,
							DateTime.Now,
							null,
							null,
							DateTime.Now,
							Constants.appWorkflowUserId);
						SetProcessParameter(Constants.ProcessParameter.TransactionQueueID, tqId.ToString());
						SaveProcessParameters();

						// Insert the MMIS Staging Tables
						RegistrationController.PopulateStagingData(tqId, CON.TransactionTypeValues.MITS, string.Empty); 
					
						ServicePointManager.Expect100Continue = true;
						ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

						txnResult = prr.providerManagementUpdateRequest(tqId, true);

                        if (txnResult.Equals(CON.TransactionResult.TransactionPassed))
						{
							nextStep = CON.TransactionResult.TransactionPassed;
							toReturn = true;
						}
						else
						{
							nextStep = CON.TransactionResult.TransactionFailed;
							toReturn = true;
						}

						TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, DateTime.Now, DateTime.Now, Constants.appWorkflowUserId);
						
						toReturn = true;
					}
				}
				else
				{
					nextStep = CON.TransactionResult.TransactionPassed;
					toReturn = true;
				}
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }
        override public string NextStep()
        {
            return nextStep;
        }                      
    }
}
