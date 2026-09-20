using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace MAXIMUS.Controllers.PDMS
{
    public interface IAuthenticationController
    {
        bool ValidateUser(string userName, string password);

        void LogAuthenticateRequest( string userName, Guid transactionId, bool isSuccess, DateTime requestTime, int responseCode, int errorCode,string errorMessage);
        
        DataSet GetProviderInfoFromUserName(string userName);
    }

    public class AuthenticationController : IAuthenticationController
    {
        public void LogAuthenticateRequest(string userName, Guid transactionId, bool isSuccess, DateTime requestTime, int responseCode, int errorCode, string errorMessage)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userName, false));
                parameters.Add(SqlParms.CreateParameter("TransactionId", DbType.Guid, transactionId, false));
                parameters.Add(SqlParms.CreateParameter("IsSuccess", DbType.Boolean, isSuccess, false));
                parameters.Add(SqlParms.CreateParameter("RequestTime", DbType.DateTime, requestTime, false));
                parameters.Add(SqlParms.CreateParameter("ResponseCode", DbType.Int32, responseCode, false));
                parameters.Add(SqlParms.CreateParameter("ErrorCode", DbType.Int32, errorCode, false));
                parameters.Add(SqlParms.CreateParameter("ErrorMessage", DbType.String, errorMessage, true));
                DataAccess.ExecuteScalar("usp_LogAuthRequest", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public DataSet GetProviderInfoFromUserName(string userName)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, userName, false));
                return DataAccess.ExecuteStoredProcedure("usp_GetProviderInfoFromUser", parameters, "UserInfo");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    
        public bool ValidateUser(string userName, string password)
        {
            try
            {
                return Membership.ValidateUser(userName, password);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
