using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;


namespace MAXIMUS.Controllers.PDMS
{
    public static class PriorAuthHospitalController
    {
        public static readonly string templateActualPath = HttpRuntime.AppDomainAppPath + @"Documents";


        public static string getFullStoredProcName(string tableName)
        {
            string storedprocname = string.Empty;
            switch (tableName.ToUpper())
            {
                case "AUTH_ATTACHMENT": storedprocname = "insertPRIOR_AUTH_ATTACHMENT"; break;
                case "AUTH_DENTAL_PROSTHODONTICS": storedprocname = "insertPRIOR_AUTH_DENTAL_PROSTHODONTICS"; break;
                case "AUTH_DENTALSAVE": storedprocname = "insertPRIOR_AUTH_DENTALSAVE"; break;
                case "AUTH_DENTALSERVICE_DETAIL": storedprocname = "insertPRIOR_AUTH_DENTALSERVICE_DETAIL"; break;
                case "AUTH_DIAGNOSIS": storedprocname = "insertPRIOR_AUTH_DIAGNOSIS"; break;
                case "AUTH_DOCUMENT_MAIL": storedprocname = "insertPRIOR_AUTH_DOCUMENT_MAIL"; break;
                case "AUTH_HOSPITAL": storedprocname = "insertPRIOR_AUTH_HOSPITAL"; break;
                case "AUTH_INSTITUTIONALSAVE": storedprocname = "insertPRIOR_AUTH_INSTITUTIONALSAVE"; break;
                case "AUTH_NOTE": storedprocname = "insertPRIOR_AUTH_Note"; break;
                case "AUTH_PROFESSIONALSAVE": storedprocname = "insertPRIOR_AUTH_PROFESSIONALSAVE"; break;
                case "AUTH_PROFFSERVICE_DETAIL": storedprocname = "insertPRIOR_AUTH_PROFFSERVICE_DETAIL"; break;
                case "AUTH_SERVICE_DETAIL": storedprocname = "insertPRIOR_AUTH_SERVICE_DETAIL"; break;
                case "DIAGNOSIS": storedprocname = "usp_SelectPRIOR_AUTH_DIAGNOSIS"; break;
                case "SERVICE_DETAIL": storedprocname = "usp_SelectPRIOR_AUTH_SERVICE_DETAIL"; break;
                case "NOTE": storedprocname = "usp_SelectPRIOR_AUTH_Note"; break;
                case "HOSPITAL": storedprocname = "usp_SelectPRIOR_AUTH_HOSPITAL"; break;
                case "DOCUMENT_MAIL": storedprocname = "usp_SelectPRIOR_AUTH_DOCUMENT_MAIL"; break;
                case "ATTACHMENT": storedprocname = "usp_SelectPRIOR_AUTH_ATTACHMENT"; break;
                case "CLAIM": storedprocname = "usp_SelectPRIOR_AUTH_CLAIM"; break;
                case "DENTAL_PROSTHODONTICS": storedprocname = "usp_SelectPRIOR_AUTH_DENTAL_PROSTHODONTICS"; break;
                case "DocumentbyMail": storedprocname = "usp_SelectPRIOR_AUTH_DocumentbyMail"; break;
                case "INSERTPRIORAUTH_NOTES": storedprocname = "usp_InsertPriorAuthNotes"; break;
                case "DELETEPRIORAUTH_NOTES": storedprocname = "usp_DeletePriorAuthNotes"; break;
                case "UPDATEPRIORAUTH_NOTES": storedprocname = "usp_UpdatePriorAuthNotes"; break;
                case "GETPRIORAUTH_NOTES": storedprocname = "usp_SelectPriorAuthNotes"; break;
                case "INSERTPRIORAUTH_DIAGNOSIS": storedprocname = "usp_InsertPriorAuthDIAGNOSIS"; break;
                case "DELETEPRIORAUTH_DIAGNOSIS": storedprocname = "usp_DeletePriorAuthDIAGNOSIS"; break;
                case "UPDATEPRIORAUTH_DIAGNOSIS": storedprocname = "usp_UpdatePriorAuthDIAGNOSIS"; break;
                case "GETPRIORAUTH_DIAGNOSIS": storedprocname = "usp_SelectPriorAuthDIAGNOSIS"; break;
                case "INSERTPRIORAUTH_DENTALSERVICEDETAILS": storedprocname = "usp_InsertPriorAuthDENTALSERVICEDETAILS"; break;
                case "DELETEPRIORAUTH_SERVICEDETAILS": storedprocname = "usp_DeletePriorAuthSERVICEDETAILS"; break;
                case "UPDATEPRIORAUTH_DENTALSERVICEDETAILS": storedprocname = "usp_UpdatePriorAuthDENTALSERVICEDETAILS"; break;
                case "GETPRIORAUTH_SERVICEDETAILS": storedprocname = "usp_SelectPriorAuthSERVICEDETAILS"; break;
                case "INSERTPRIORAUTH_PROFFSERVICEDETAILS": storedprocname = "usp_InsertPriorAuthPROFFSERVICEDETAILS"; break;
                case "UPDATEPRIORAUTH_PROFFSERVICEDETAILS": storedprocname = "usp_UpdatePriorAuthPROFFSERVICEDETAILS"; break;
                case "INSERTPRIORAUTH_INSTSERVICEDETAILS": storedprocname = "usp_InsertPriorAuthINSTSERVICEDETAILS"; break;
                case "UPDATEPRIORAUTH_INSTSERVICEDETAILS": storedprocname = "usp_UpdatePriorAuthINSTSERVICEDETAILS"; break;
            }
            return storedprocname;
        }


        public static int InsertPriorAuthHospital(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertUpdatePriorAuthPanelData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void DeletePriorAuthPanelData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetPriorAuthPanelData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure(getFullStoredProcName(tableName), parameters, "PrioData_GetPriorAuthPanelData");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet SelectPriorAuthHospital(int PriorAuthHospitalID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_HOSPITAL_ID", DbType.Int32, PriorAuthHospitalID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_HOSPITAL", parameters, "PrioData_" + PriorAuthHospitalID);

                lookup.Tables[0].TableName = "PrioData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPriorAuthDiagnosis(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectPriorAuthDiagnosis(int PriorAuthDiagnosisID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_ID", DbType.Int32, PriorAuthDiagnosisID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_DIAGNOSIS", parameters, "DiaData_" + PriorAuthDiagnosisID);

                lookup.Tables[0].TableName = "DiaData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPriorAuthServiceDetail(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectPriorAuthServiceDetail(int PriorAuthServiceDetailID)
        {

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_ID", DbType.Int32, PriorAuthServiceDetailID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_SERVICE_DETAIL", parameters, "ServiceDetailData_" + PriorAuthServiceDetailID);

                lookup.Tables[0].TableName = "ServiceDetailData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPriorAuthDentalProsthodontics(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectPriorAuthDentalProsthodontics(int PriorAuthDentalProsthodonticsID)
        {


            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DENTAL_PROSTHODONTICS_ID", DbType.Int32, PriorAuthDentalProsthodonticsID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPPRIOR_AUTH_DENTAL_PROSTHODONTICS", parameters, "ServiceDetailData_" + PriorAuthDentalProsthodonticsID);

                lookup.Tables[0].TableName = "DentalProsthodontics";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPrioHospitalData(string tableName, int PriohospitalId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_HOSPITAL_ID", DbType.String, PriohospitalId, true));

                DataSet ds;
                ds = DataAccess.ExecuteStoredProcedure(getFullStoredProcName(tableName), parameters, "PrioHospital_" + PriohospitalId);
                ds.Tables[0].TableName = "PrioHospital";
                return ds;
            }

            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        // Submit claim (no stored proc; need to remove this in future)
        public static DataSet GetSubmitClaimData(string tableName, int Submitclaimid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SUBMIT_CLAIM_ID", DbType.String, Submitclaimid, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure(getFullStoredProcName(tableName), parameters, "SubmitClaim" + Submitclaimid);
                ds.Tables[0].TableName = "SubmitClaim";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static int InsertPriorAuthNote(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectPriorAuthNote(int PriorAuthNoteID, string medicaidId)
        {


            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_ID", DbType.Int32, PriorAuthNoteID, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidId", DbType.Int32, medicaidId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_Note", parameters, "NoteData_" + PriorAuthNoteID);

                lookup.Tables[0].TableName = "Note";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPriorAuthAttachment(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }
        //public static DataSet SelectPriorAuthAttachment(int PriorAuthAttachmentID)

        public static int InsertPriorAuthDocumentByMail(string tableName, Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, object> pair in parms)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(pair.Value)))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertPriorAuthDentalRecord(string tableName, Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, object> pair in parms)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(pair.Value)))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void UpdatePriorAuthDentalServiceDetailsRecord(string authType, Dictionary<string, object> parms)
        {
            string procName = default(string);

            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> pair in parms)
            {
                if (pair.Value == null)
                {
                    parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                }
                else
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
            }

            try
            {
                switch (authType)
                {
                    case "Institutional":
                        procName = "updatePRIOR_AUTH_SERVICE_DETAIL";
                        break;
                    case "Dental":
                        procName = "updatePRIOR_AUTH_DENTALSERVICE_DETAIL";
                        break;
                    case "Professional":
                        procName = "updatePRIOR_AUTH_PROFFSERVICE_DETAIL";
                        break;
                }
                DataAccess.ExecuteStoredProcedure(procName, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Updating Prior Auth Service details for " + authType + "  -  " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static bool checkIfRecordExistsServiceDetailDental(int ServiceDetailID)
        {
            try
            {
                bool isExists = false;
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ServiceDetailID", DbType.Int32, ServiceDetailID, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_checkIfRecordExistsServiceDetailDental", parameters, "checkIfRecordExistsServiceDetailDental");
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int getTopMostServiceDetailDental()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_getTopMostServiceDetailDental", parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int insertPRIOR_AUTH_PROFFSERVICE_DETAIL(string tableName, Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();


                foreach (KeyValuePair<string, object> pair in parms)
                {
                    Type _type = pair.Value.GetType();
                    if (_type.Equals(typeof(string)))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(pair.Value)))
                        {
                            parameters.Add(new SqlParameter(pair.Key, pair.Value));
                        }
                        else
                        {
                            parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                        }
                    }
                    else
                    {
                        if (pair.Value != null)
                        {
                            parameters.Add(new SqlParameter(pair.Key, pair.Value));
                        }
                        else
                        {
                            parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                        }
                    }
                }
                //int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar("insertPRIOR_" + tableName, parameters));
                string scalerId = DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters);


                return !string.IsNullOrEmpty(scalerId) ? 1 : 0;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet SelectPriorAuthDocumentByMail(int PriorAuthDocumentByMailID, int RegID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_HOSPITAL_ID", DbType.Int32, PriorAuthDocumentByMailID, false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_DOCUMENT_MAIL", parameters, "DocumentByMailData_" + PriorAuthDocumentByMailID);

                lookup.Tables[0].TableName = "DocumentByMail";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchBenefitResponse()
        {
            try
            {
                DataSet ds = new DataSet();
                //ds.ReadXml(templateActualPath + @"/InquireRecipientEligibilitySoapResponse.xml");

                //string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                //ds.ReadXml(templateActualPath + @"/RecipientEligibilityResponse.xml");
                // ds.ReadXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\InquireRecipientEligibilitySoapResponse.xml");
                //ds.ReadXml(@"D:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\RecipientEligibilityResponse.xml");
                ds.ReadXml(@"D:\Projects\GitHub\ohpnm-src\PDMS\PDMS\Documents\RecipientEligibilityResponse.xml");
                return ds;


            }

            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SearchBenefitRequest()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(templateActualPath + @"/RecipientEligibilityRequest.xml");
                //   ds.ReadXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\RecipientEligibilityRequest.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetSubmitPriorAuthRequestResponse()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(templateActualPath + @"/SubmitPriorAuthRequestResponse.xml");

                //ds.ReadXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\SubmitPriorAuthRequestResponse.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SearchPriorAuthResponse()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(templateActualPath + @"/SearchAuthorizationResponse.xml", XmlReadMode.Auto);

                //ds.ReadXml(@"D:\src\ohpnm - src\PDMS\ProviderDataManagementSystemService\Documents\SearchAuthorizationResponse.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        // OHPNM-5582 AP SearchPA Page 
        public static DataSet SearchPRIORAUTHTRACKING(string trackingNumber, int statustype, int? payerId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("trackingNumber", DbType.String, trackingNumber, true));
                parameters.Add(SqlParms.CreateParameter("statustype", DbType.Int32, statustype, false));
                parameters.Add(SqlParms.CreateParameter("payerid", DbType.Int32, payerId, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchPRIOR_AUTH_TRACKING", parameters, "searchSaved_prior_auth");

                ds.Tables[0].TableName = "searchSaved_prior_auth";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchPRIORAUTHTRACKINGByMedID(string MedicaidID, string trackingNumber, int statustype, int? payerId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, MedicaidID, true));
                parameters.Add(SqlParms.CreateParameter("trackingNumber", DbType.String, trackingNumber, true));
                parameters.Add(SqlParms.CreateParameter("statustype", DbType.Int32, statustype, false));
                parameters.Add(SqlParms.CreateParameter("payerid", DbType.Int32, payerId, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_SearchPRIOR_AUTH_TRACKING", parameters, "searchSaved_prior_auth");

                ds.Tables[0].TableName = "searchSaved_prior_auth";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchPriorTrackingSubmitPA(string medicaidId, string trackingNo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaid_id", DbType.String, medicaidId, true));
                parameters.Add(SqlParms.CreateParameter("tracking_no", DbType.String, trackingNo, true));

                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_Get_SAVED_PRIOR_AUTH_BY_MEDICAID_ID", parameters, "Saved_prior_authMedicaid");

                ds.Tables[0].TableName = "Saved_prior_authMedicaid";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int GetPriorAuthDetailSaveID(int paType, string medicaidId, string trackingNo)
        {
            int statusId = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medicaidId, false));
            parameters.Add(SqlParms.CreateParameter("PATIENT_TRACKING_NUMBER", DbType.String, trackingNo, false));
            parameters.Add(SqlParms.CreateParameter("PA_TYPE", DbType.Int32, paType, false));
            try
            {
                statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_Detail_SaveID", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
            return statusId;
        }

        public static DataSet GetUpdatePriorAuthRequestResponse()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(templateActualPath + @"/UpdatePriorAuthRequestResponse.xml");

                //ds.ReadXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\UpdatePriorAuthRequestResponse.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void InsertOrUpdateInquirePriorAuth(string xmlPriorAuthResponse)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetAddUPdateClaimsRequestResponse()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(templateActualPath + @"/AddUPdateClaimsRequestResponse.xml");

                // ds.ReadXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\AddUPdateClaimsRequestResponse.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet DentalSearch()
        {
            try
            {
                DataSet ds = new DataSet();
                if (File.Exists(templateActualPath + @"/Dentalsearch.xml"))
                {
                    ds.ReadXml(templateActualPath + @"/Dentalsearch.xml");
                }

                // ds.ReadXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\Dentalsearch.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthDocumentMail(string tableName, int hospitalId, int regId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_HOSPITAL_ID", DbType.String, hospitalId, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, regId, true));
                string storedproc = string.Empty;

                switch (tableName.ToLower())
                {
                    case "diagnosis": storedproc = "usp_SelectPRIOR_AUTH_DIAGNOSIS"; break;
                    case "service_detail": storedproc = "usp_SelectPRIOR_AUTH_SERVICE_DETAIL"; break;
                    case "note": storedproc = "usp_SelectPRIOR_AUTH_NOTE"; break;
                    case "hospital": storedproc = "usp_SelectPRIOR_AUTH_HOSPITAL"; break;
                    case "document_mail": storedproc = "usp_SelectPRIOR_AUTH_DOCUMENT_MAIL"; break;
                    case "attachment": storedproc = "usp_SelectPRIOR_AUTH_ATTACHMENT"; break;
                    case "claim": storedproc = "usp_SelectPRIOR_AUTH_CLAIM"; break;
                    case "dental_prosthodontics": storedproc = "usp_SelectPRIOR_AUTH_DENTAL_PROSTHODONTICS"; break;
                    case "documentbymail": storedproc = "usp_SelectPRIOR_AUTH_DocumentbyMail"; break;
                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName : " + tableName);
                }
                DataSet ds = DataAccess.ExecuteStoredProcedure(storedproc, parameters, "PrioHospital_" + hospitalId);
                ds.Tables[0].TableName = "DOCUMENT_MAIL";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static CorrespodenceInfo GetSearchCorrespondenceType(int correspondeceType, Guid userId, DateTime? fromDate, DateTime? toDate, int regId, string npi, string medicaidId, string sortOrder, string currentSortField, int pageSize = 0, int pageIndex = 0)//OHPNM-3814
        {
            var data = new CorrespodenceInfo();
            string outValue1;
            string outValue2;
            try
            {
                #region CORRESPODENCE INFO
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(SqlParms.CreateParameter("CorrespondeceType", DbType.Int32, correspondeceType, false));
                parameters1.Add(SqlParms.CreateParameter("userId", DbType.Guid, userId, false));
                parameters1.Add(SqlParms.CreateParameter("regId", DbType.Int32, regId, false));
                parameters1.Add(SqlParms.CreateParameter("npi", DbType.String, npi, false));
                parameters1.Add(SqlParms.CreateParameter("medicaidId", DbType.String, medicaidId, false));
                parameters1.Add(SqlParms.CreateParameter("fromDate", DbType.DateTime, fromDate, true));
                parameters1.Add(SqlParms.CreateParameter("toDate", DbType.DateTime, toDate, true));
                parameters1.Add(SqlParms.CreateParameter("SortDirection", DbType.String, sortOrder, false));
                parameters1.Add(SqlParms.CreateParameter("SortField", DbType.String, currentSortField, false));
                DataSet ds1 = DataAccess.ExecuteStoredProcedure("usp_Get_Correspondence_Details", parameters1, "ResultCount", SqlDbType.Int, out outValue1, 0);
                if (ObjectControllerHelper.HasRows(ds1))
                {
                    ds1.Tables[0].TableName = "CorrespondenceResults";
                    if (!string.IsNullOrEmpty(outValue1))
                    {
                        data.CorrespondenceResultCount = Convert.ToInt32(outValue1);
                    }
                    data.CorrespondenceInfo = ds1;
                }

                #endregion

                #region CONVERTED CORRESPODENCE INFO
                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(SqlParms.CreateParameter("CorrespondeceType", DbType.Int32, correspondeceType, false));
                parameters2.Add(SqlParms.CreateParameter("regId", DbType.Int32, regId, false));
                parameters2.Add(SqlParms.CreateParameter("fromDate", DbType.DateTime, fromDate, false));
                parameters2.Add(SqlParms.CreateParameter("toDate", DbType.DateTime, toDate, false));
                parameters2.Add(SqlParms.CreateParameter("SortDirection", DbType.String, sortOrder, false));
                parameters2.Add(SqlParms.CreateParameter("SortField", DbType.String, currentSortField, false));
                var ds2 = DataAccess.ExecuteStoredProcedure("usp_Get_ConvertedCorrespondence_Details", parameters2, "ResultCount", SqlDbType.Int, out outValue2, 0);

                if (correspondeceType == Constants.correspondeceType)
                {
                    //ds1.Tables[0].Columns.Add("DOCUMENT_ID", typeof(string));
                    ds1.Tables[0].Columns.Add("ELIGIBILITY", typeof(string));
                }
                if (ObjectControllerHelper.HasRows(ds2))
                {
                    ds2.Tables[0].TableName = "ConvertedCorrespondenceResults";

                    if (!string.IsNullOrEmpty(outValue2))
                    {
                        data.ConvertedCorrespondenceResultCount = Convert.ToInt32(outValue2);
                    }
                    data.ConvertedCorrespondenceInfo = ds2;
                }
                #endregion

                return data;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetAssignement(string prioAuthorizationType)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PrioAuthorizationType", DbType.String, prioAuthorizationType, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_Assignment_Type", parameters, "Assignment_Type");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        #region Diagonis details
        public static int UpdatePriorDiagnosisServiceDetail(int? SequenceId, int? diagCodeTypeId, string diagCode, string diagCodeDesc, string lastModifiedBy, string diagnosisDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_ID", DbType.Int32, SequenceId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", DbType.Int32, diagCodeTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_CODE", DbType.String, diagCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_DESC", DbType.String, diagCodeDesc, true));

                //parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_INSTITUTIONALSAVE_ID", DbType.Int32, 0, true));
                //parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DENTALSAVE_ID", DbType.Int32, 0, true));
                //parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_PROFESSIONALSAVE_ID", DbType.Int32, 0, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_STATUS", DbType.Int32, 0, false));

                if (string.IsNullOrEmpty(diagnosisDate))
                {
                    parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_DATE", DbType.DateTime, System.DateTime.Now, true));
                }
                else
                {
                    parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_DATE", DbType.DateTime, Convert.ToDateTime(diagnosisDate), true));
                }
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedBy, false));

                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("updatePRIOR_AUTH_DIAGNOSIS", parameters));

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int InsertPriorDiagnosisServiceDetail(int? SequenceId, int? diagCodeTypeId, string diagCode, string diagCodeDesc, string diagnosisDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_ID", DbType.Int32, SequenceId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", DbType.Int32, diagCodeTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_CODE", DbType.String, diagCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_DESCRIPTION", DbType.String, diagCodeDesc, true));
                if (string.IsNullOrEmpty(diagnosisDate))
                {
                    parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_DATE", DbType.DateTime, System.DateTime.Now, true));
                }
                else
                {
                    parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_DATE", DbType.DateTime, Convert.ToDateTime(diagnosisDate), true));
                }
                //DataAccess.ExecuteStoredProcedure("usp_InsertPRIOR_AUTH_Diagnosis", parameters);
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertPRIOR_AUTH_DIAGNOSIS", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorDiagnosisServiceDetail(int? priorAuthtype, string medicaid = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_TYPE", DbType.Int32, priorAuthtype, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaid, false));
                return DataAccess.ExecuteStoredProcedure("usp_selectPRIOR_AUTH_DIAGNOIS", parameters, "PRIOR_AUTH_DIAGNOIS");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static void DeletePriorDiagnosisServiceDetail(int lineNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DIAGNOSIS_ID", DbType.Int32, lineNumber, false));
                DataAccess.ExecuteStoredProcedure("usp_DeletePRIOR_AUTH_Diagnosis", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int GetPriorAuthDiagnosisSaveID(string priorAuthType)
        {
            int statusId = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_TYPE", DbType.String, priorAuthType, false));
                statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_Diagnosis_SaveID", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
            return statusId;
        }
        #endregion


        #region Service Details

        public static int InsertPriorAuthServiceDetail(int? codeTypeId, string revenueCode, string procedureCode, string reqUnits, int? unitMeasurementId,
            decimal? reqUnitsFee, DateTime reqFDOS, DateTime reqTDOS, int? statusId, string procCodeDesc, string providerServiceNote, int? levelofCareId,
            int? authUnits, int? remainigUnits, decimal? authDollars, DateTime? authTDOS, DateTime? authFDOS, int? serviceTrackingNo, DateTime lastModifiedDateTime,
            Guid lastModifiedBy, DateTime? createdDate, Guid? createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", DbType.Int32, codeTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_REVENUE_CODE", DbType.String, revenueCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", DbType.String, procedureCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", DbType.String, reqUnits, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_REQUESTED_UNITS_ID", DbType.Int32, unitMeasurementId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", DbType.Decimal, reqUnitsFee, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", DbType.DateTime, reqFDOS, false));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", DbType.DateTime, reqTDOS, false));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_STATUS_ID", DbType.Int32, statusId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", DbType.String, procCodeDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", DbType.String, providerServiceNote, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_LEVEL_CARE_ID", DbType.Int32, levelofCareId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", DbType.String, authUnits, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", DbType.Int32, remainigUnits, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", DbType.Decimal, authDollars, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", DbType.DateTime, authTDOS, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", DbType.DateTime, authFDOS, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", DbType.Int32, serviceTrackingNo, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("Created_On_Date_Time", DbType.DateTime, createdDate, true));
                parameters.Add(SqlParms.CreateParameter("Created_By_User", DbType.Guid, createdBy, true));

                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertPRIOR_AUTH_SERVICE_DETAIL", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int UpdatePriorAuthServiceDetail(int detailedId, int? codeTypeId, string revenueCode, string procedureCode, string reqUnits, int? unitMeasurementId, decimal? reqUnitsFee,
          DateTime reqFDOS, DateTime reqTDOS, int? statusId, string procCodeDesc, string providerServiceNote, int? levelofCareId, int? authUnits, int? remainingUnits, decimal? authDollars, DateTime? authTDOS,
          DateTime? authFDOS, int? serviceTrackingNo, DateTime lastModifiedDateTime, Guid lastModifiedBy, DateTime? createdDate, Guid? createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_ID", DbType.Int32, detailedId, false));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", DbType.Int32, codeTypeId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_REVENUE_CODE", DbType.String, revenueCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", DbType.String, procedureCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", DbType.String, reqUnits, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_REQUESTED_UNITS_ID", DbType.Int32, unitMeasurementId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", DbType.Decimal, reqUnitsFee, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", DbType.DateTime, reqFDOS, false));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", DbType.DateTime, reqTDOS, false));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_STATUS_ID", DbType.Int32, statusId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", DbType.String, procCodeDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", DbType.String, providerServiceNote, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_LEVEL_CARE_ID", DbType.Int32, levelofCareId, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", DbType.String, authUnits, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", DbType.Int32, remainingUnits, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", DbType.Decimal, authDollars, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", DbType.DateTime, authTDOS, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", DbType.DateTime, authFDOS, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", DbType.Int32, serviceTrackingNo, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedDateTime, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastModifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("Created_On_Date_Time", DbType.DateTime, createdDate, true));
                parameters.Add(SqlParms.CreateParameter("Created_By_User", DbType.Guid, createdBy, true));

                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("updatePRIOR_AUTH_SERVICE_DETAIL", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetPriorAuthServiceDetail(string MedicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, MedicaidId, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_SERVICE_DETAIL", parameters, "Service_Details");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePriorAuthServiceDetail(int lineNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_SERVICE_DETAIL_ID", DbType.Int32, lineNumber, false));
                DataAccess.ExecuteStoredProcedure("DeletePRIOR_AUTH_SERVICE_DETAIL", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int CreatePriorAuthProviderNotes(string notesType, string noteDesc, string noteStatus, string noteReasonCodeDesc,
            string revProvDesc, string authType, string reasonCode, Guid? createdBy = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Notes_Types", DbType.String, notesType, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_PROVIDER_DESC", DbType.String, noteDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_STATUS", DbType.String, noteStatus, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_REVIEWERProvider_DESC", DbType.String, revProvDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_REASON_CODE_DESC", DbType.String, noteReasonCodeDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_REASON_CODE_MMIS", DbType.String, reasonCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_AUTH_TYPE", DbType.String, authType, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, createdBy, true));


                //PRIOR_AUTH_Notes_Types                    notesType
                //PRIOR_AUTH_Note_PROVIDER_DESC             noteDesc
                //PRIOR_AUTH_Note_STATUS                    noteStatus
                //PRIOR_AUTH_Note_REVIEWERProvider_DESC     revProvDesc
                //PRIOR_AUTH_Note_REASON_CODE_DESC          noteReasonCodeDesc
                //PRIOR_AUTH_REASON_CODE_MMIS               reasonCode
                //PRIOR_AUTH_Note_AUTH_TYPE                 authType
                //CREATED_BY_USER


                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("insertPRIOR_AUTH_Note", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static int UpdatePriorAuthProviderNotes(Int32 noteid, string notesType, string noteDesc, string noteStatus,
          string noteReasonCodeDesc, string revProvDesc, DateTime lastModifiedBy, string prior_Auth_Type, string reasonCode, Guid? createdBy = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_ID", DbType.Int32, noteid, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Notes_Types", DbType.String, notesType, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_PROVIDER_DESC", DbType.String, noteDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_STATUS", DbType.String, noteStatus, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_REVIEWERProvider_DESC", DbType.String, revProvDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_REASON_CODE_DESC", DbType.String, noteReasonCodeDesc, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_REASON_CODE_MMIS", DbType.String, reasonCode, true));
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_Note_AUTH_TYPE", DbType.String, prior_Auth_Type, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastModifiedBy, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, false));

                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("updatePRIOR_AUTH_Note", parameters));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthProviderNotes(string authType, string medicaid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_TYPE", DbType.String, authType, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidId", DbType.String, medicaid, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_Note", parameters, "PRIOR_AUTH_PROVIDER_NOTE");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePriorAuthProviderNote(int noteid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_NOTE_ID", DbType.Int32, noteid, false));
                DataAccess.ExecuteStoredProcedure("usp_DeletePRIOR_AUTH_Note", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePriorAuthAttachment(int documentId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentId, false));
                DataAccess.ExecuteStoredProcedure("usp_DeletePRIOR_AUTH_ATTACHMENT", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectPriorAuthAttachment(string priorAuthType, string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_ATTACHMENT_AUTH_TYPE", DbType.String, priorAuthType, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidId", DbType.String, medicaidId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_ATTACHMENTByAuthType", parameters, "AttachmentData_" + priorAuthType);

                lookup.Tables[0].TableName = "Attachment";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectClaimsAttachment(string priorAuthType, string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ATTACHMENT_AUTH_TYPE", DbType.String, priorAuthType, false));
                parameters.Add(SqlParms.CreateParameter("MedicaidId", DbType.String, medicaidId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_ATTACHMENTByAuthType", parameters, "AttachmentData_" + priorAuthType);

                lookup.Tables[0].TableName = "Attachment";

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        #endregion

        #region Dental and Professional

        public static DataSet GetPriorAuthDocumentTypes()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DENTAL_DOCUMENT_TYPE", "PRIOR_AUTH_DOCUMENT_TYPES");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthDentalServiceDetail(string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, medicaidId, false));
                return DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_DENTALSERVICE_DETAIL", parameters, "PRIOR_AUTH_DENTAL_SERVICE_DETAILS");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthProfessionalServiceDetail(string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, medicaidId, false));
                return DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_PROFFSERVICE_DETAIL", parameters, "PRIOR_AUTH_PROFESSIONAL_SERVICE_DETAILS");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePriorAuthDentalServiceDetail(int lineNum)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DENTALSERVICE_DETAIL_ID", DbType.Int32, lineNum, false));
                DataAccess.ExecuteStoredProcedure("DeletePRIOR_AUTH_DENTALSERVICE_DETAIL", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePriorAuthProfessionalServiceDetail(int lineNum)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_PROFFSERVICE_DETAIL_ID", DbType.Int32, lineNum, false));
                DataAccess.ExecuteStoredProcedure("DeletePRIOR_AUTH_PROFFSERVICE_DETAIL", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthDentalOralCavity()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_DENTAL_ORAL_CAVITY", "PRIOR_AUTH_DENTAL_ORAL_CAVITY");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthDentalProsthesis()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY", "PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthDentalToothSurface()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_DENTALTOOTH_SURFACE", "AUTH_DENTALTOOTH_SURFACE");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorAuthDentalToothNumber()
        {
            try
            {
                return DataAccess.ExecuteStoredProcedure("usp_SelectPrior_auth_submit_tooth_number", "AUTH_DENTALTOOTH_NUMBER");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPriorAuthServiceDetails(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(getFullStoredProcName(tableName), parameters));
                return newAddId;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void UpdatePriorAuthServiceDetails(string authType, Dictionary<string, string> parms)
        {
            string procName = default(string);

            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (KeyValuePair<string, string> pair in parms)
            {
                if (pair.Value == null)
                {
                    parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                }
                else
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
            }

            try
            {
                switch (authType)
                {
                    case "Institutional":
                        procName = "updatePRIOR_AUTH_SERVICE_DETAIL";
                        break;
                    case "Dental":
                        procName = "updatePRIOR_AUTH_DENTALSERVICE_DETAIL";
                        break;
                    case "Professional":
                        procName = "updatePRIOR_AUTH_PROFFSERVICE_DETAIL";
                        break;
                }
                DataAccess.ExecuteStoredProcedure(procName, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Updating Prior Auth Service details for " + authType + "  -  " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertPriorAuthNotes(Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar("insertPRIOR_AUTH_Note", parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }



        #endregion

        #region Prior Auth Saving

        public static int GetPriorAuthAttachmentSaveID(string priorAuthType)
        {
            try
            {
                int num = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_TYPE", DbType.String, priorAuthType, false));
                DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_Attachment_SaveID", parameters);
                return num;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int GetPriorAuthServiceDetailSaveID(string priorAuthType, string medicaidId)
        {
            int statusId = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, medicaidId, false));
            try
            {
                switch (priorAuthType.ToUpper())
                {
                    case "INSTITUTIONAL":
                        statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_ServiceDetail_SaveID", parameters));
                        break;
                    case "DENTAL":
                        statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_DentalServiceDetail_SaveID", parameters));
                        break;
                    case "PROFESSIONAL":
                        statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_ProfessionalServiceDetail_SaveID", parameters));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
            return statusId;
        }

        public static int GetPriorAuthServiceDetailLineID(string priorAuthType, string medicaidId)
        {
            int statusId = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, medicaidId, false));
            try
            {
                switch (priorAuthType.ToUpper())
                {
                    case "INSTITUTIONAL":
                        statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_ServiceDetail_LineID", parameters));
                        break;
                    case "DENTAL":
                        statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_DentalServiceDetail_LineID", parameters));
                        break;
                    case "PROFESSIONAL":
                        statusId = Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPRIOR_AUTH_ProfessionalServiceDetail_LineID", parameters));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
            return statusId;
        }

        public static bool ValidateReqProvEnrollStatus(string RequestProviderMedID)
        {
            try
            {
                bool isExists = false;
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("RequestProviderMedID", DbType.String, RequestProviderMedID, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_CheckReqProvEnrollStatus_Active", parameters, "ServiceProvider");
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool ValidateSvcProvEnrollStatus(string ServiceProvider)
        {
            try
            {
                bool isExists = false;
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("ServiceCode", DbType.String, ServiceProvider, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_CheckServProvEnrollStatus_Active", parameters, "ServiceProvider");
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetSearchAuthData(string PayerName, string AssignmentType, string DiagnosisCode, string ICDProcedureCode,
                    string MemberMedicaidId, string OrderingProviderID, string PriorAuthNumber, string CPTHCPCSServiceCode,
                    string RevenueCode, string PatientEventTrackingNumber, DateTime? SubDate, DateTime? PAEffDate, DateTime? PAExpDate,
                    DateTime? DateOfBirth, string PAStatus)
        {
            DataSet data = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PayerName", DbType.String, PayerName, false));
                parameters.Add(SqlParms.CreateParameter("AssignmentType", DbType.String, AssignmentType, true));
                parameters.Add(SqlParms.CreateParameter("DiagnosisCode", DbType.String, DiagnosisCode, true));
                parameters.Add(SqlParms.CreateParameter("ICDProcedureCode", DbType.String, ICDProcedureCode, true));
                parameters.Add(SqlParms.CreateParameter("MemberMedicaidId", DbType.String, MemberMedicaidId, true));
                parameters.Add(SqlParms.CreateParameter("OrderingProviderID", DbType.String, OrderingProviderID, true));
                parameters.Add(SqlParms.CreateParameter("PriorAuthNumber", DbType.String, PriorAuthNumber, true));
                parameters.Add(SqlParms.CreateParameter("CPTHCPCSServiceCode", DbType.String, CPTHCPCSServiceCode, true));
                parameters.Add(SqlParms.CreateParameter("RevenueCode", DbType.String, RevenueCode, true));
                parameters.Add(SqlParms.CreateParameter("PatientEventTrackingNumber", DbType.String, PatientEventTrackingNumber, true));
                parameters.Add(SqlParms.CreateParameter("SubDate", DbType.DateTime, SubDate, true));
                parameters.Add(SqlParms.CreateParameter("PAEffDate", DbType.DateTime, PAEffDate, true));
                parameters.Add(SqlParms.CreateParameter("PAExpDate", DbType.DateTime, PAExpDate, true));
                parameters.Add(SqlParms.CreateParameter("DateOfBirth", DbType.DateTime, DateOfBirth, true));
                parameters.Add(SqlParms.CreateParameter("PAStatus", DbType.String, PAStatus, true));

                data = DataAccess.ExecuteStoredProcedure("usp_Search_Auth_data", parameters, "SearchAuthData");

                return data;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetServProvDateOfService(string npi,string medicaidID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ServiceProviderNPI", DbType.String, npi, false));
                parameters.Add(SqlParms.CreateParameter("ServiceProviderMedicaidID", DbType.String, medicaidID, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_Service_Provider_DateofService", parameters, "MedicaidServiceDate");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }       

        public static DataSet GetOrdProvDateOfService(string medicaidID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, medicaidID, false));
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_Ordering_Provider_DateofService", parameters, "MedicaidOrderingDate");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        #endregion

        public static int GetPriorAuthAssignmentProcGrp(string PAAssignCode, string procFrom)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CDE_PA_ASSIGN", DbType.String, PAAssignCode, false));
                parameters.Add(SqlParms.CreateParameter("PROC_FROM", DbType.String, procFrom, false));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_SelectPA_ASSIGN_PROCEDURE_GRP", parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Guid savePAMainRecord(string medID, int PAType, string paTrackNumber)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, medID, false));
                parameters.Add(SqlParms.CreateParameter("PATIENT_TRACKING_NUMBER", DbType.String, paTrackNumber, false));
                parameters.Add(SqlParms.CreateParameter("PA_TYPE", DbType.Int32, PAType, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(Constants.appAdminUserId), false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.String, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, new Guid(Constants.appAdminUserId), false));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.String, DateTime.Now, false));
                return new Guid(DataAccess.ExecuteScalar("InsertUpdatePA_MAIN_INFORMATION", parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SavePriorAuth(int authType, Dictionary<string, object> parms)
        {
            string procName = default(string);

            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> pair in parms)
            {
                if (pair.Value == null)
                {
                    parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                }
                else
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
            }

            try
            {
                switch (authType)
                {
                    case Constants.PAClaimsType.Institutional:
                        procName = "InsertUpdatePRIOR_AUTH_SUB_INSTSAVE";
                        break;
                    case Constants.PAClaimsType.Dental:
                        procName = "InsertUpdatePRIOR_AUTH_SUB_DENTALSAVE";
                        break;
                    case Constants.PAClaimsType.Professional:
                        procName = "InsertUpdatePRIOR_AUTH_SUB_PROFSAVE";
                        break;
                }
                DataAccess.ExecuteScalar(procName, parameters);
                return 0;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Updating Prior Auth Service details for " + authType + "  -  " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertPriorAuthDiagnosisData(string tableName, Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, object> pair in parms)
                {
                    if (pair.Value == null)
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(tableName, parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static int InsertPriorAuthServiceDetailsData(string tableName, Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, object> pair in parms)
                {
                    if (pair.Value == null)
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(tableName, parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetPriorDiagnosisDetailsBySectionID(Guid lnkSections)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LINK_SECTION", DbType.Guid, lnkSections, false));
                return DataAccess.ExecuteStoredProcedure("usp_selectPRIOR_AUTH_DIAGNOISByPAIDs", parameters, "PRIOR_AUTH_DIAGNOISGUID");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetPriorServiceDetailsBySectionID(int PA_TYPE, Guid lnkSections)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_TYPE", DbType.Guid, PA_TYPE, false));
                parameters.Add(SqlParms.CreateParameter("LINK_SECTION", DbType.Guid, lnkSections, false));
                return DataAccess.ExecuteStoredProcedure("usp_selectPRIOR_AUTH_ServiceDetailsByPAIDs", parameters, "PRIOR_AUTH_SERVICEGUID");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static int InsertPriorAuthAttachmentData(string tableName, Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, object> pair in parms)
                {
                    if (pair.Value == null)
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                int newAddId = Convert.ToInt32(DataAccess.ExecuteScalar(tableName, parameters));
                return newAddId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetPriorAttachmentsBySectionID(int PA_TYPE, Guid lnkSections)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_TYPE", DbType.Int32, PA_TYPE, false));
                parameters.Add(SqlParms.CreateParameter("LINK_SECTION", DbType.Guid, lnkSections, false));
                return DataAccess.ExecuteStoredProcedure("usp_selectPRIOR_AUTH_SUB_AttachmentByPAIDs", parameters, "PRIOR_AUTH_ATTACHGUID");
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void UpdateViewed(string documentId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter(
                    "DocumentId",
                    DbType.String,
                    documentId,
                    false
                ));

                DataAccess.ExecuteStoredProcedure("usp_UpdateViewedDate", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static CorrespodenceInfo GetSearchCorrespondenceType(int correspondeceType, Guid userId,
         DateTime? fromDate, DateTime? toDate, int? regId, string npi, string medicaidId,
         string sortOrder, string currentSortField, int pageSize = 0, int pageIndex = 0)//OHPNM-3814
        {
            var data = new CorrespodenceInfo();
            string outValue1;
            string outValue2;
            try
            {
                #region CORRESPODENCE INFO
                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(SqlParms.CreateParameter("CorrespondeceType", DbType.Int32, correspondeceType, false));
                parameters1.Add(SqlParms.CreateParameter("userId", DbType.Guid, userId, false));
                parameters1.Add(SqlParms.CreateParameter("regId", DbType.Int32, regId, true));
                parameters1.Add(SqlParms.CreateParameter("npi", DbType.String, npi, true));
                parameters1.Add(SqlParms.CreateParameter("medicaidId", DbType.String, medicaidId, true));
                parameters1.Add(SqlParms.CreateParameter("fromDate", DbType.DateTime, fromDate, true));
                parameters1.Add(SqlParms.CreateParameter("toDate", DbType.DateTime, toDate, true));
                parameters1.Add(SqlParms.CreateParameter("SortDirection", DbType.String, sortOrder, false));
                parameters1.Add(SqlParms.CreateParameter("SortField", DbType.String, currentSortField, false));
                DataSet ds1 = DataAccess.ExecuteStoredProcedure("usp_Get_Correspondence_Details", parameters1, "ResultCount", SqlDbType.Int, out outValue1, 0);
                if (ObjectControllerHelper.HasRows(ds1))
                {
                    ds1.Tables[0].TableName = "CorrespondenceResults";
                    if (!string.IsNullOrEmpty(outValue1))
                    {
                        data.CorrespondenceResultCount = Convert.ToInt32(outValue1);
                    }
                    data.CorrespondenceInfo = ds1;
                }

                #endregion

                #region CONVERTED CORRESPODENCE INFO
                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(SqlParms.CreateParameter("CorrespondeceType", DbType.Int32, correspondeceType, false));
                parameters2.Add(SqlParms.CreateParameter("regId", DbType.Int32, regId, false));
                parameters2.Add(SqlParms.CreateParameter("fromDate", DbType.DateTime, fromDate, false));
                parameters2.Add(SqlParms.CreateParameter("toDate", DbType.DateTime, toDate, false));
                parameters2.Add(SqlParms.CreateParameter("SortDirection", DbType.String, sortOrder, false));
                parameters2.Add(SqlParms.CreateParameter("SortField", DbType.String, currentSortField, false));
                var ds2 = DataAccess.ExecuteStoredProcedure("usp_Get_ConvertedCorrespondence_Details", parameters2, "ResultCount", SqlDbType.Int, out outValue2, 0);

                if (correspondeceType == Constants.correspondeceType)
                {
                    DataTable table = ds1.Tables[0];

                    if (!table.Columns.Contains("ELIGIBILITY"))
                    {
                        table.Columns.Add("ELIGIBILITY", typeof(string));
                    }
                }
                if (ObjectControllerHelper.HasRows(ds2))
                {
                    ds2.Tables[0].TableName = "ConvertedCorrespondenceResults";

                    if (!string.IsNullOrEmpty(outValue2))
                    {
                        data.ConvertedCorrespondenceResultCount = Convert.ToInt32(outValue2);
                    }
                    data.ConvertedCorrespondenceInfo = ds2;
                }
                #endregion

                return data;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
