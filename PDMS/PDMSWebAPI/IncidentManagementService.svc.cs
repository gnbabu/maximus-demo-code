using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.IncidentManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IncidentReport" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select IncidentReport.svc or IncidentReport.svc.cs at the Solution Explorer and start debugging.
    public class IncidentManagementService : IIncidentManagementService
    {
        Logging Log;
        Guid IncidentServiceID = new Guid(CON.WebApiServiceGuid.IncidentServiceSend);
        public IncidentManagementService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                Log = new Logging(IncidentServiceID, "PDMSWebAPI:IncidentReport");
            }
            else
            {
                Log = new Logging(user.UserThreadId, "PDMSWebAPI:IncidentReport");
            }
        }

        //public string HealthCheck()
        //{
        //    return "OK";
        //}

        public sendProviderIncidentResponse SendProviderIncident(sendProviderIncidentRequest1 Incident)
        {
            Log.CreateLogEntry("Calling SendProviderIncident", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:IncidentReport", "WebAPI:IncidentReport-sendProviderIncidentResponse");

            SendProviderIncidentRequest Payload = Incident.Payload.SendProviderIncidentInfo;
            sendProviderIncidentResponse rtn = new sendProviderIncidentResponse();
            string eMsg = "";

            rtn.SITransactionKey = Incident.MessageHeader.SITransactionKey;
           
            try
            {
                if (Payload.MedicaidProviderID.Trim() == "")
                {
                    rtn.ResponseCode = "3002";
                    rtn.ResponseMessage = "Parsing error has occurred: Medicaid Provider Id is required";
                    Log.CreateLogEntry("Error in SendProviderIncident: " + eMsg, Logging.LogPriority.Error);
                    return rtn;
                }
                else
                {
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, Payload.MedicaidProviderID.Trim(), true));
                    DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationsByMedicaidID", param, "RegDetails");
                    if (!Methods.HasRows(dsReg))
                    {
                        rtn.ResponseCode = "3002";
                        rtn.ResponseMessage = "Medicaid Provider Id not exist in PNM.";
                        Log.CreateLogEntry("Error in SendProviderIncident: " + eMsg, Logging.LogPriority.Error);
                        return rtn;
                    }
                }
                if (string.IsNullOrEmpty(Payload.IMSCaseNumber))
                {
                    rtn.ResponseCode = "3002";
                    rtn.ResponseMessage = "Incident Case number cannot be blank";
                    Log.CreateLogEntry("Error in SendProviderIncident: " + eMsg, Logging.LogPriority.Error);
                    return rtn;
                }
                foreach (SendProviderIncidentRequestIncidentDetails INode in Payload.IncidentDetails)
                {
                    // validate first
                   
                    if (string.IsNullOrEmpty(INode.IncidentID))
                    {
                        rtn.ResponseCode = "3002";
                        rtn.ResponseMessage = "Incident ID cannot be blank.";
                        Log.CreateLogEntry("Error in SendProviderIncident: " + eMsg, Logging.LogPriority.Error);
                        return rtn;
                    }

                    // create parameters objects and fill with values
                    string spName = "usp_SaveIncidentReport";
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, Payload.MedicaidProviderID, false));
                    parameters.Add(SqlParms.CreateParameter("PROVIDER_NAME", DbType.String, Payload.ProviderName, false));
                    parameters.Add(SqlParms.CreateParameter("INCIDENT_CASE_NUMBER", DbType.String, Payload.IMSCaseNumber, false));
                    parameters.Add(SqlParms.CreateParameter("CASE_SUBMITTED_DATE", DbType.DateTime, Payload.NODReferralDate, true));
                    parameters.Add(SqlParms.CreateParameter("CASE_STATUS", DbType.Int32, 1, false));  // TODO: NEED CASE STATUS
                    parameters.Add(SqlParms.CreateParameter("INCIDENT_CATEGORY", DbType.String, INode.IncidentCategory, false));
                    parameters.Add(SqlParms.CreateParameter("INCIDENT_SUB_CATEGORY", DbType.String, INode.IncidentSubcategory, false));
                    parameters.Add(SqlParms.CreateParameter("INCIDENT_TYPE", DbType.String, INode.IncidentType, false));
                    parameters.Add(SqlParms.CreateParameter("INCIDENT_ID", DbType.String, INode.IncidentID, false));
                    parameters.Add(SqlParms.CreateParameter("SUBSTANTIATED_UNSUBSTANTIATED", DbType.String, INode.Substantiated, false));
                    parameters.Add(SqlParms.CreateParameter("NOD_REFERRAL_DATE", DbType.DateTime, Payload.NODReferralDate, true));
                    parameters.Add(SqlParms.CreateParameter("NOD_REASON", DbType.String, Payload.ReasonForNOD, false));
                    parameters.Add(SqlParms.CreateParameter("REFERRAL_SENT_BY", DbType.String, Payload.ReferralSentBy, false));
                    parameters.Add(SqlParms.CreateParameter("IMS_ASSOCIATE_ID", DbType.String, INode.IMSAssociateID, false));
                    parameters.Add(SqlParms.CreateParameter("NOD_ISSUED_DATE", DbType.DateTime, Payload.NODReferralDate, true));
                    parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, DBNull.Value, true));
                    parameters.Add(SqlParms.CreateParameter("SI_TRANS_KEY", DbType.String, Incident.MessageHeader.SITransactionKey, true));

                    DataSet ds = new DataSet();
                    ds = DataAccess.ExecuteStoredProcedure(spName, parameters, "RS");
                    
                    string rslt = ds.Tables[0].Rows[0][0].ToString();  // returns "OK" but proc will throw error if there is a problem, so no need to check result
                                        
                }

            }
            catch (Exception ex)
            {
                rtn.ResponseCode = "500"; //error
                rtn.ResponseMessage = ex.Message;
                rtn.ResponseType = SendProviderIncidentResponseResponseType.Failure;
                SaveIncidentReqRes(Incident, rtn);
                Log.CreateLogEntry("Error in SendProviderIncident: " + ex.Message, Logging.LogPriority.Error);
                return rtn;
            }

            // if we get here, all is good
            rtn.ResponseCode = "200"; //good
            rtn.ResponseMessage = "OK";
            rtn.ResponseType = SendProviderIncidentResponseResponseType.Success;
            SaveIncidentReqRes(Incident, rtn);
            Log.CreateLogEntry("SendProviderIncident Successful", Logging.LogPriority.Information);

            return rtn;
        }

        private void SaveIncidentReqRes(sendProviderIncidentRequest1 req, sendProviderIncidentResponse rtn)
        {
            try
            {

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(sendProviderIncidentRequest1));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, req, emptyNs);
                string Reqxml = stream2.ToString();

                System.Xml.Serialization.XmlSerializer xres = new System.Xml.Serialization.XmlSerializer(typeof(sendProviderIncidentResponse));
                var emptyNsRes = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settingsRes = new XmlWriterSettings();
                settingsRes.Indent = true;
                settingsRes.OmitXmlDeclaration = true;

                var stream1 = new StringWriter();
                var writer1 = XmlWriter.Create(stream1, settingsRes);
                xres.Serialize(writer1, rtn, emptyNsRes);
                string Resxml = stream1.ToString();

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(SqlHelper.CreateParameter("INCIDENT_CASE_NUMBER", DbType.String, req.Payload.SendProviderIncidentInfo.IMSCaseNumber.ToString(), false));
                param.Add(SqlHelper.CreateParameter("REQUEST_FOR", DbType.String, "SendProviderIncidentRequest", false));
                param.Add(SqlHelper.CreateParameter("REQUEST", DbType.String, Reqxml.ToString(), false));
                param.Add(SqlHelper.CreateParameter("RESPONSE", DbType.String, Resxml.ToString(), false));
                param.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                param.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, false));
                InfoAccess.ExecuteScalar("insertINCIDENT_MANAGEMENT_SERVICE_REQ_RES", param);
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry("Error in Inbound IMS saving INCIDENT_MANAGEMENT_SERVICE_REQ_RES: " + ex.Message, Logging.LogPriority.Error);
                throw ex;
            }
        }

    }

}
