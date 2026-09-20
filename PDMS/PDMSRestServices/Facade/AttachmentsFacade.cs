using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using PDMSRestServices.Controllers.Facade;
using PDMSRestServices.Models;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSRestServices.Facade
{
    public class AttachmentsFacade
    {
        private const string SENDER_ID = "MMISODJFS";
        private const string ATTACHMENT_FILE_SUFFIX = "AT";

        public static int SelectPriorAuthAttachment(string claimType, string medicaidId)
        {
            var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(claimType, medicaidId);
            var dataTable = ds.Tables["Attachments"];

            return dataTable?.Rows.Count ?? 0; // Return the cou
        }

        public static int SavePAUploadedFile(AttachmentUploadRequest request)
        {
            int documentId = 0;

            try
            {
                // Clean file path and generate identifiers
                string fileName = HelperFacade.CleanFilePath(request.FileName);
                string tradingPartnerID = request.TradingPartnerID;
                string attachmentFileName = GenerateAttachmentFileName(Guid.NewGuid());
                string fileExtension = Path.GetExtension(request.FileName);
                string outboundIdentifier = GenerateDocumentNumber(request.MedicaidId);

                // Generate new file name
                string newFileName = $"{attachmentFileName}{request.Timestamp}--{ATTACHMENT_FILE_SUFFIX}--{tradingPartnerID}{fileExtension}";

                // Adjust new file name for dental claims
                if (request.ClaimType.Equals("dental", StringComparison.OrdinalIgnoreCase))
                {
                    newFileName = $"{attachmentFileName}{request.Timestamp}--{ATTACHMENT_FILE_SUFFIX}--{tradingPartnerID}{fileExtension}";
                }

                // Call service to save the uploaded file

                documentId = LookupTableController.InsertPAOutBoundDocumentUploads(
                    CON.BillingServicetypeId.PA,
                    request.PayerRequested,
                    request.MemberId,
                    request.ClaimTypeId,
                    request.ClaimNumber,
                    request.PaNumber,
                    request.ProviderId,
                    request.ProviderNPI,
                    SENDER_ID,
                    request.ReceiverId,
                    request.DocumentTypeId,
                    request.ApiFileName,
                    request.UUID,
                    request.ToSend,
                    new Guid(CON.appAdminUserId),
                    outboundIdentifier,
                    request.OriginalDocumentName
                );

                return documentId;
            }
            catch (Exception ex)
            {
                throw new Exception($"File entries not saved. Error: {HelperFacade.HtmlEncode(ex.Message)}");
            }
        }

        public static string GenerateDocumentNumber(string medicaidId)
        {
            string providerNPI = string.Empty;

            DataSet ds = RegistrationController.SelectProviderByGRPMedicaidID(medicaidId);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                providerNPI = Helper.GetString("NPI", dr);
            }
            return providerNPI + DateTime.Now.ToString(" yyyy-mm-dd hh:mm:ss");
        }

        public static string GenerateAttachmentFileName(Guid uuid)
        {
            DateTime dateTime = DateTime.Now;
            return string.Format("{0}--{1}--{2}-", SENDER_ID, uuid, dateTime.ToString("yyyyMMdd"));
        }

        public static DataSet CheckClaimIsDelet(int claimid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Adjudication_Summary_Status_ScreenData", parameters, "claim_Adjudication_Summary_Status_Screen");

                return lookup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<AttachmentModel> GetAttachmentsByMedicaid(string medicaidId, string clmNum, string clmType)
        {
            var result = new List<AttachmentModel>();

            if (string.IsNullOrWhiteSpace(medicaidId))
                return result;

            var ds = LookupTableController.GetUploadAttachmentsByMedicaid(medicaidId, clmNum, clmType);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                result.Add(new AttachmentModel
                {
                    OutBound_Document_Uploads_ID = Convert.ToInt32(row["OutBound_Document_Uploads_ID"]),
                    DocumentName = row["DocumentName"]?.ToString(),
                    Claim_number = row["Claim_number"]?.ToString(),
                    PA_NUMBER = row["PA_NUMBER"]?.ToString(),
                    Member_ID = row["Member_ID"]?.ToString(),
                    DOCUMENT_TYPE_DESC = row["DOCUMENT_TYPE_DESC"]?.ToString(),
                    OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER = row["OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER"]?.ToString()
                });
            }

            return result;
        }


        public static void DeleteAttachmentById(int documentId, bool isUnsent = false)
        {
            var param = new List<SqlParameter>
            {
                SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, documentId, true)
            };

            if (!isUnsent)
            {
                DataAccess.ExecuteStoredProcedure("usp_DeleteOutboundDocumentUpload", param);
            }
            else
            {
                DataAccess.ExecuteStoredProcedure("usp_DeleteUnsentOutboundDocumentUpload", param);
            }
        }

        public static bool SaveStandaloneAttachment(AttachmentPayloadModel model)
        {

            string payerRequested = "Yes";
            bool to_send = false;
            string outboundIdentifier = GenerateDocumentNumber(model.MedicaidId);

            DocumentUploadController.InsertOutBoundDocumentUploads(
                Convert.ToInt32(model.EDITransactionTypeId),
                payerRequested,
                model.MemberId,
                Convert.ToInt32(model.ClaimTypeId),
                model.ClaimNumber,
                model.PaNumber,
                model.ProviderId,
                model.Provider_NPI,
                SENDER_ID,
                model.ReceiverId,
                Convert.ToInt32(model.DocumentTypeId),
                model.DocumentName,
                Guid.NewGuid(),
                to_send,
                new Guid(CON.appAdminUserId),
                outboundIdentifier,
                model.OrginalDocumentName,
                model.ProviderComments
            );

            return true;
        }

        public static bool InsertClaimsOutBoundDocumentUploads(AttachmentPayloadModel model)
        {

            string payerRequested = "No";
            bool to_send = false;
            string outboundIdentifier = GenerateDocumentNumber(model.MedicaidId);

            DocumentUploadController.InsertClaimsOutBoundDocumentUploads(
                Convert.ToInt32(model.EDITransactionTypeId),
                payerRequested,
                model.MemberId,
                Convert.ToInt32(model.ClaimTypeId),
                model.ClaimNumber,
                model.PaNumber,
                model.ProviderId,
                model.Provider_NPI,
                SENDER_ID,
                model.ReceiverId,
                Convert.ToInt32(model.DocumentTypeId),
                model.DocumentName,
                Guid.NewGuid(),
                to_send,
                new Guid(CON.appAdminUserId),
                outboundIdentifier,
                model.OrginalDocumentName,
                model.DocumentId
            );

            return true;
        }


        public static bool UpdateDocumentStatuses(List<int> documentIds)
        {
            try
            {
                var processAttachments = new ProcessAttachments();
                // Update each document status
                foreach (int docId in documentIds)
                {
                    processAttachments.UpdateFileStatusToSend(docId, 0);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool DeleteUnSentAttachment(List<int> documentIds)
        {
            try
            {
                foreach (int docId in documentIds)
                {
                    DeleteAttachmentById(docId, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
