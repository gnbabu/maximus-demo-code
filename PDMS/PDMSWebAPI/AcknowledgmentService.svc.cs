using MAXIMUS.Core.Libraries;
using PDMSWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PartialAcknowledgement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PartialAcknowledgement.svc or PartialAcknowledgement.svc.cs at the Solution Explorer and start debugging.
    public class AcknowledgmentService : IAcknowledgmentService
    {
        Logging Log;

        public AcknowledgmentService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                Log = new Logging(Guid.NewGuid(), "PDMSWebAPI:AcknowledgmentService");
            }
            else
            {
                Log = new Logging(user.UserThreadId, "PDMSWebAPI:AcknowledgmentService");
            }
        }

        public void TargetVendorResponse(TargetVendorResponse acknowledgementModel)
        {
            Log.CreateLogEntry(string.Format("Calling TargetVendorResponse for Requestor System ID {0}", acknowledgementModel.RequestorSystemId), Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:AcknowledgmentService", "WebAPI:AcknowledgmentService-TargetVendorResponse");

            string appPDMSDataExchangeUserId = "FD41DDFC-DBD6-4BFD-BCB2-27B3BB31D211";
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("RequestorSystem", DbType.String, acknowledgementModel.RequestorSystem, true));

            parameters.Add(SqlParms.CreateParameter("RequestorSystemId", DbType.String, acknowledgementModel.RequestorSystemId, true));
            parameters.Add(SqlParms.CreateParameter("TargetSystem", DbType.String, acknowledgementModel.TargetSystem , true));
            parameters.Add(SqlParms.CreateParameter("TargetSystemId", DbType.String, acknowledgementModel.TargetSystemId, true));
            parameters.Add(SqlParms.CreateParameter("SITransactionKey", DbType.String, acknowledgementModel.SITransactionKey, true));
            parameters.Add(SqlParms.CreateParameter("ModuleTransactionId", DbType.String, acknowledgementModel.ModuleTransactionId, true));

            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("insertStg_Message_Header", parameters);

            parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("TRANSACTION_QUEUE_ID", DbType.Int32, acknowledgementModel.ModuleTransactionId, true));

            
            
            parameters.Add(SqlParms.CreateParameter("PROCESS_DATE_TIME", DbType.DateTime, DateTime.Now , true));
            parameters.Add(SqlParms.CreateParameter("ACK_TimeStamp", DbType.DateTime, acknowledgementModel.TimeStamp, true));
            parameters.Add(SqlParms.CreateParameter("SI_RESPONSE_CODE", DbType.Int32, acknowledgementModel.ResponseCode, true));
            parameters.Add(SqlParms.CreateParameter("SI_RESPONSE_MESSAGE", DbType.String, acknowledgementModel.ResponseMessage, true));
            parameters.Add(SqlParms.CreateParameter("SI_RESPONSE_TYPE", DbType.String, acknowledgementModel.ResponseType , true));
            parameters.Add(SqlParms.CreateParameter("SI_AdditionalModuleTransactionID", DbType.String, acknowledgementModel.AdditionalModuleTransactionId , true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("usp_SaveTransaction", parameters);

            Log.CreateLogEntry("TargetVendorResponse success", Logging.LogPriority.Information);
        }

        public string EchoSoapRequest(int input)
        {
            var rawRequest = OperationContext.Current.RequestContext.RequestMessage.ToString();

            return rawRequest;
        }
    }
}
