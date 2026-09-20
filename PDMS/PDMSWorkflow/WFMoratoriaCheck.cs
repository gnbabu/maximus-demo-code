using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFMoratoriaCheck : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFMoratoriaCheck(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            string returnVal = string.Empty; 
            Process p = new Process(ProcessID);

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
                int matchResult = 0;

                //Match check only applicable for new, non-converted providers.  Match proc will return 0 if existing provider.
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                sqlParms.Add(SqlParms.CreateParameter("ModifiedOn", DbType.DateTime, DateTime.Now, false));
                sqlParms.Add(SqlParms.CreateParameter("ModifiedBy", DbType.Guid, Constants.appAdminUserId, false));
                returnVal = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_PerformMoratoria_MatchForProvider", sqlParms, "MatchResult", SqlDbType.Int));

                matchResult = string.IsNullOrEmpty(returnVal) ? 0 : Convert.ToInt32(returnVal);

                switch (matchResult)
                {
                    case (int)Enumerations.MoratoriaMatchResult.NoMatch:
                        nextStep = "Next";
                        break;
                    case (int)Enumerations.MoratoriaMatchResult.MatchFound:
                        nextStep = "Move To Exclusion";
                        break;
                    default:
                        nextStep = "Next";
                        break;
                }
                toReturn = true;
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
