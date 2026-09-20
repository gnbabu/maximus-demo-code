using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PDMSWebAPI.CostReports
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CostReports" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CostReports.svc or CostReports.svc.cs at the Solution Explorer and start debugging.
    public class CostReports : ICostReports
    {
        Logging Log;
        public CostReports()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                Log = new Logging(Guid.NewGuid(), "PDMSWebAPI:CostReportsService");
            }
            else
            {
                Log = new Logging(user.UserThreadId, "PDMSWebAPI:CostReportsService");
            }
        }
        // public cSubmitCostSettlementReportResponse submitCostSettlementReport(MessageHeader MessageHeader, cSubmitCostSettlementReport Payload)        // public cSubmitCostSettlementReportResponse submitCostSettlementReport(submitCostSettlementReportRequest Request)
        public submitCostSettlementReportResponse submitCostSettlementReport(submitCostSettlementReportRequest settlementRequest)
        {
            MessageHeader MessageHeader = settlementRequest.MessageHeader;
            cSubmitCostSettlementReport Payload = settlementRequest.Payload;

            Log.CreateLogEntry("Calling submitCostSettlementReport", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:CostReports", "WebAPI:CostReports-submitCostSettlementReport");

            bool vldPassed = true;
            string eMsg = "";
            
            cSubmitCostSettlementReportResponse xc = new cSubmitCostSettlementReportResponse();
            submitCostSettlementReportResponse rtn = new submitCostSettlementReportResponse(xc);
            cSIResponse rsp = new cSIResponse();
            rtn.ResponsePayload.Response = rsp;
            rtn.ResponsePayload.Response.SITransactionKey = MessageHeader.SITransactionKey;
            rtn.ResponsePayload.Response.ResponseType = "500";
            rtn.ResponsePayload.Response.ResponseMessage = "";

            try
            {

                // validate first
                if (Payload.IdProvider.Trim() == "")
                {
                    vldPassed = false;
                    eMsg = "Parsing error has occurred: Medicaid Provider Id is required";
                }

                if (vldPassed == false)  // something went wrong
                {
                    rtn.ResponsePayload.Response.ResponseMessage = eMsg;
                    Log.CreateLogEntry("Error in SendProviderIncident: " + eMsg, Logging.LogPriority.Error);
                    return rtn;
                }

                // parse and store settlement report
                string spName = "usp_SaveCostSettlementReport";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, MessageHeader.SITransactionKey, false));
                parameters.Add(SqlParms.CreateParameter("PROVIDER_ID", DbType.String, Payload.IdProvider, false));
                parameters.Add(SqlParms.CreateParameter("COST_REPORT_TYPE", DbType.String, Payload.CostReportType, false));
                parameters.Add(SqlParms.CreateParameter("COST_REPORT_FROM_DATE", DbType.DateTime, Payload.CostReportFromDate, true));
                parameters.Add(SqlParms.CreateParameter("COST_REPORT_TO_DATE", DbType.DateTime, Payload.CostReportToDate, true));
                parameters.Add(SqlParms.CreateParameter("COST_REPORT_FISCAL_YEAR", DbType.String, Payload.CostReportFiscalYear, false));
                parameters.Add(SqlParms.CreateParameter("SUMBISSION_DATE", DbType.DateTime, Payload.SubmissionDate, true));
                parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, DBNull.Value, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure(spName, parameters, "RS");
                int CostSettlementReportId = int.Parse(ds.Tables[0].Rows[0][0].ToString());  // returns new CostSettlementReportId but proc will throw error if there is a problem, so no need to check result

                // parse and store attachements (only 1 attachement allowed)
                rtn.ResponsePayload.AttachmentData = new cAttachmentData[Payload.AttachmentInfo.Length];
                int DocID = 0;
                int ptr = 0;
                foreach (cSubmitCostSettlementReportAttachmentInfo a in Payload.AttachmentInfo)
                {
                    spName = "usp_SaveCostSettlementReportAttachement";
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("COST_SETTLEMENT_REPORT_ID", DbType.Int32, CostSettlementReportId, false));
                    parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE", DbType.String, a.DocumentType, false));
                    parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, a.DocumentName, false));
                    parameters.Add(SqlParms.CreateParameter("FIRST_ACCESS_DATE", DbType.DateTime, a.FirstAccessDate, true));
                    parameters.Add(SqlParms.CreateParameter("READ_INDICATOR", DbType.String, a.ReadIndicator, false));
                    parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, DBNull.Value, true));
                    ds = DataAccess.ExecuteStoredProcedure(spName, parameters, "RS");
                    DocID = int.Parse(ds.Tables[0].Rows[0][0].ToString());  // returns new DocId but proc will throw error if there is a problem, so no need to check result
                    rtn.ResponsePayload.AttachmentData[ptr] = new cAttachmentData();
                    rtn.ResponsePayload.AttachmentData[ptr].AttachmentID = DocID.ToString();
                    rtn.ResponsePayload.AttachmentData[ptr].AttachmentType = a.DocumentType;
                }

            }
            catch (Exception ex)
            {
                rtn.ResponsePayload.Response.ResponseType = "500";
                rtn.ResponsePayload.Response.ResponseMessage = ex.Message;
                return rtn;
            }

            rtn.ResponsePayload.Response.ResponseType = "200";
            rtn.ResponsePayload.Response.ResponseMessage = "Success";
            return rtn;
        }
    }
}
