using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PDMSWebAPI.Models;
using MAXIMUS.Core.Libraries;
using System.Data;
using System.Data.SqlClient;


namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PartialAcknowledgement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PartialAcknowledgement.svc or PartialAcknowledgement.svc.cs at the Solution Explorer and start debugging.
    public class PartialAcknowledgement : IPartialAcknowledgement
    {
        public void TargetVendorResponse(TargetVendorResponse acknowledgementModel)
        {
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
            parameters.Add(SqlParms.CreateParameter("TimeStamp", DbType.DateTime, acknowledgementModel.TimeStamp, true));
            parameters.Add(SqlParms.CreateParameter("MMIS_STATUS_CODE", DbType.Int32, acknowledgementModel.ResponseCode, true));
            parameters.Add(SqlParms.CreateParameter("RESPONSE_MESSAGE", DbType.String, acknowledgementModel.ResponseMessage, true));
            parameters.Add(SqlParms.CreateParameter("Response_Type", DbType.String, acknowledgementModel.ResponseType , true));
            parameters.Add(SqlParms.CreateParameter("Additional_Module_Transaction_ID", DbType.String, acknowledgementModel.AdditionalModuleTransactionId , true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appPDMSDataExchangeUserId, true));

            DataAccess.ExecuteStoredProcedure("usp_SaveTransaction", parameters);
        }


    }
}
