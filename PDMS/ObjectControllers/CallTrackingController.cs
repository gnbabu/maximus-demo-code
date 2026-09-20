using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public static class CallTrackingController
    {
        public static int InsertCallTrackingCall(int sourceID, int subjectID, int nextActionID, int resolutionID, DateTime startTime, DateTime endTime, TimeSpan duration, string regID,
            string callerOther, string reasonOther, string callDetails, string NPI, string medicaidID, DateTime createdOn, Guid createdBy)
        {
            int newID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SourceID", DbType.Int32, sourceID, true));
                parameters.Add(SqlParms.CreateParameter("SubjectID", DbType.Int32, subjectID, true));
                parameters.Add(SqlParms.CreateParameter("NextActionID", DbType.Int32, nextActionID, true));
                parameters.Add(SqlParms.CreateParameter("ResolutionId", DbType.Int32, resolutionID, true));

                parameters.Add(SqlParms.CreateParameter("StartTime", DbType.DateTime, startTime, true));
                parameters.Add(SqlParms.CreateParameter("EndTime", DbType.DateTime, endTime, true));
                parameters.Add(new SqlParameter("Duration", duration));
                //parameters.Add(SqlParms.CreateParameter("UserID", DbType.String, createdBy, true));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.String, regID, true));
                //parameters.Add(SqlParms.CreateParameter("MemberID", DbType.String, tbd, true));
                //parameters.Add(SqlParms.CreateParameter("JudicialDistrict", DbType.String, tbd, true));
                //parameters.Add(SqlParms.CreateParameter("CaseWorkerName", DbType.String, tbd, true));
                parameters.Add(SqlParms.CreateParameter("CallerOther ", DbType.String, callerOther, true));
                parameters.Add(SqlParms.CreateParameter("ReasonOther ", DbType.String, reasonOther, true));
                parameters.Add(SqlParms.CreateParameter("CallDetails", DbType.String, callDetails, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID, true));
                parameters.Add(SqlParms.CreateParameter("CreatedOn ", DbType.DateTime, createdOn, true));
                parameters.Add(SqlParms.CreateParameter("CreatedBy ", DbType.Guid, createdBy, true));

                newID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertCALLTRACKING_CALL", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            return newID;
        }

        public static DataSet SelectCallsByRange(DateTime beginDate, DateTime endDate, int sourceID, int subjectID, int nextActionID, int resolutionID, int callID, string NPI, string medicaidID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("BeginDate", DbType.Date, beginDate, true));
                parameters.Add(SqlParms.CreateParameter("EndDate", DbType.Date, endDate, true));
                if (sourceID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("SourceID", DbType.Int32, sourceID, true));
                }
                if (subjectID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("SubjectID", DbType.Int32, subjectID, true));
                }
                if (nextActionID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("NextActionID", DbType.Int32, nextActionID, true));
                }
                if (resolutionID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("ResolutionId", DbType.Int32, resolutionID, true));
                }
                if (callID > 0)
                {
                    parameters.Add(SqlParms.CreateParameter("CallID", DbType.Int32, callID, true));
                }
                if (NPI != null && NPI.Trim() != string.Empty)
                {
                    parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                }
                if (medicaidID != null && medicaidID.Trim() != string.Empty)
                {
                    parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID, true));
                }

                DataSet ds = new DataSet();
                string storedProc = "usp_SelectCALLTRACKING_CALLS_BYRANGE";
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, "CallData");
                ds.Tables[0].TableName = "CallData";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
   
}
