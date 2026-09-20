using Corp.Core.Libraries.AttachmentServiceReference;
using Corp.Core.Libraries.FinancialServiceReference;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AttachmentMessageHeader = Corp.Core.Libraries.AttachmentServiceReference.MessageHeader;
using FinancialMessageHeader = Corp.Core.Libraries.FinancialServiceReference.MessageHeader;

namespace MAXIMUS.Controllers.PDMS
{
    public class ServiceAgentController
    {
        public static List<AttachmentResponse> SendAttachments(List<SendAttachment> sendAttachments, AttachmentMessageHeader messageHeader)
        {
            try
            {
                SendAttachmentPayload attachmentPayload = new SendAttachmentPayload
                {
                    AttachmentInfo = new SendAttachmentInformation
                    {
                        AttachmentData = sendAttachments.ToArray()
                    }
                };
                AttachmentSoapPortTypeClient attachmentService = new AttachmentSoapPortTypeClient();
                string moduleTraansactionid="";
                string additionalModuleTransactionId="";
                AttachmentResponse[] attachmentResponses = null ;
                var response = attachmentService.sendAttachment(messageHeader, attachmentPayload, out moduleTraansactionid,out additionalModuleTransactionId, out attachmentResponses);
                return (attachmentResponses != null)? attachmentResponses.ToList() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static ServiceResponse CreateDocument(CreateDocumentInformation documentInformation, DocumentMessageHeader messageHeader)
        //{
        //    try
        //    {
        //        CreateDocumentPayload createDocumentPayload = new CreateDocumentPayload
        //        {
        //            DocumentInformation = documentInformation
        //        };
        //        DocumentService documentService = new DocumentService();
        //        string moduleTransactionId;
        //        string responseCode;
        //        string responseType;
        //        string oDSProviderDemographicsId;
        //        string responseMessage;
        //        string responseDetails;
        //       documentService.createDocument(messageHeader, createDocumentPayload, out moduleTransactionId, out responseCode, out responseType, out oDSProviderDemographicsId, out responseMessage, out responseDetails);
        //        ServiceResponse response = new ServiceResponse()
        //        {
        //            ModuleTransactionId = moduleTransactionId,
        //            ResponseCode = responseCode,
        //            ODSProviderDemographicsId = oDSProviderDemographicsId,
        //            ResponseDetails = responseDetails,
        //            ResponseMessage = responseMessage,
        //            ResponseType = responseType
        //        };

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static ServiceResponse SendProviderEnrollment(int transactionQueueID, ProviderManagementMessageHeader messageHeader)
        //{
        //    DataSet ds = new DataSet();
        //    //Read data from Db and convert dataset into EnrollRequestPayload
        //    ds = StagingDataController.GetStagingProviderEnrollment(transactionQueueID);
        //    EnrollRequestPayload enrollRequestPayload = new EnrollRequestPayload();
        //    enrollRequestPayload.ProviderInformation = new EnrollProviderInformation
        //    {
        //        ProviderAddress = DatatableHelper.ConvertDataTableToList<EnrollProviderInformationAddress>(ds.Tables[0]).ToArray(),
        //        OwnerRelationship = DatatableHelper.ConvertDataTableToList<OwnerRelationshipListRelationship>(ds.Tables[1]).ToArray(),
        //        ProviderAffiliations = DatatableHelper.ConvertDataTableToList<ProviderAffiliationListAffiliations>(ds.Tables[2]).ToArray(),
        //        ProviderAlternateIdentifiers = DatatableHelper.ConvertDataTableToList<ProviderAlternateIdListAlternateIdentifier>(ds.Tables[3]).ToArray(),
        //        ProviderEFTEnrollment = DatatableHelper.ConvertDataTableToList<EFTEnrollmentListEFTEnrollment>(ds.Tables[4]).ToArray(),
        //        ProviderLanguage = DatatableHelper.ConvertDataTableToList<ProviderLanguageListLanguages>(ds.Tables[5]).ToArray(),
        //        ProviderDemographics = DatatableHelper.GetItem<EnrollProviderDemographics>(ds.Tables[6].Rows[0]),
        //        ProviderDocuments = DatatableHelper.ConvertDataTableToList<ProviderDocumentsListDocuments>(ds.Tables[7]).ToArray(),
        //        ProviderApplication = DatatableHelper.ConvertDataTableToList<enrollProviderApplication>(ds.Tables[8]).ToArray(),
        //        ProviderAttestations = DatatableHelper.ConvertDataTableToList<ProviderAttestationsListAttestations>(ds.Tables[9]).ToArray(),
        //        ProviderBusinessStatus = DatatableHelper.ConvertDataTableToList<ProviderBusinessStatusListBusinessStatus>(ds.Tables[10]).ToArray(),
        //        ProviderCHOP = DatatableHelper.ConvertDataTableToList<ProviderCHOPListCHOP>(ds.Tables[11]).ToArray(),
        //        ProviderContact = DatatableHelper.ConvertDataTableToList<ProviderContactListContact>(ds.Tables[12]).ToArray(),
        //        ProviderManagedEmployees = DatatableHelper.ConvertDataTableToList<ProviderManagedEmployeesListManagedEmployee>(ds.Tables[13]).ToArray(),
        //        ProviderOwnership = DatatableHelper.ConvertDataTableToList<ProviderOwnershipList>(ds.Tables[14]).ToArray(),
        //        ProviderProgramAffiliations = DatatableHelper.ConvertDataTableToList<ProgramAffiliationsListProgramAffiliation>(ds.Tables[15]).ToArray(),
        //        ProviderReviews = DatatableHelper.ConvertDataTableToList<ProviderReviewsListReview>(ds.Tables[16]).ToArray(),
        //        ProviderServiceLocation = DatatableHelper.ConvertDataTableToList<EnrollProviderServiceLocation>(ds.Tables[17]).ToArray(),
        //        ProviderServices = DatatableHelper.ConvertDataTableToList<ProviderServicesListServices>(ds.Tables[18]).ToArray(),
        //        ProviderTaxonomyClassification = DatatableHelper.ConvertDataTableToList<TaxonomyClassificationListTaxonomyClassification>(ds.Tables[19]).ToArray(),
        //        ProviderType = DatatableHelper.ConvertDataTableToList<ProviderTypeListType>(ds.Tables[20]).ToArray()
        //    };

        //    string moduleTransactionId;
        //    string responseCode;
        //    string responseType;
        //    string oDSProviderDemographicsId;
        //    string responseMessage;
        //    string responseDetails;
        //    ProviderManagementService providerManagementService = new ProviderManagementService();
        //    providerManagementService.enrollProvider(messageHeader, enrollRequestPayload, out moduleTransactionId, out responseCode, out responseType, out oDSProviderDemographicsId, out responseMessage, out responseDetails);
        //    ServiceResponse enrollmentResponse = new ServiceResponse()
        //    {
        //        ModuleTransactionId = moduleTransactionId,
        //        ResponseCode = responseCode,
        //        ODSProviderDemographicsId = oDSProviderDemographicsId,
        //        ResponseDetails = responseDetails,
        //        ResponseMessage = responseMessage,
        //        ResponseType = responseType
        //    };
        //    return enrollmentResponse;
        //}

        //public static ServiceResponse UpdateProviderEnrollment(DataSet ds, ProviderManagementMessageHeader messageHeader)
        //{
        //    ProviderManagementService providerManagementService = new ProviderManagementService();
        //    UpdateRequestPayload updateRequestPayload = new UpdateRequestPayload();
        //    updateRequestPayload.ProviderInformation = new UpdateProviderInformation
        //    {
        //        ProviderAddress = DatatableHelper.ConvertDataTableToList<ProviderAddressListAddress>(ds.Tables[0]).ToArray(),
        //        OwnerRelationship = DatatableHelper.ConvertDataTableToList<OwnerRelationshipListRelationship>(ds.Tables[1]).ToArray(),
        //        ProviderAffiliations = DatatableHelper.ConvertDataTableToList<ProviderAffiliationListAffiliations>(ds.Tables[2]).ToArray(),
        //        ProviderAlternateIdentifiers = DatatableHelper.ConvertDataTableToList<ProviderAlternateIdListAlternateIdentifier>(ds.Tables[3]).ToArray(),
        //        ProviderEFTEnrollment = DatatableHelper.ConvertDataTableToList<EFTEnrollmentListEFTEnrollment>(ds.Tables[4]).ToArray(),
        //        ProviderLanguage = DatatableHelper.ConvertDataTableToList<ProviderLanguageListLanguages>(ds.Tables[5]).ToArray(),
        //        ProviderDemographics = DatatableHelper.GetItem<UpdateProviderInformationProviderDemographics>(ds.Tables[6].Rows[0]),
        //        ProviderDocuments = DatatableHelper.ConvertDataTableToList<ProviderDocumentsListDocuments>(ds.Tables[7]).ToArray(),
        //        ProviderApplication = DatatableHelper.ConvertDataTableToList<updateProviderApplication>(ds.Tables[8]).ToArray(),
        //        ProviderAttestations = DatatableHelper.ConvertDataTableToList<ProviderAttestationsListAttestations>(ds.Tables[9]).ToArray(),
        //        ProviderBusinessStatus = DatatableHelper.ConvertDataTableToList<ProviderBusinessStatusListBusinessStatus>(ds.Tables[10]).ToArray(),
        //        ProviderCHOP = DatatableHelper.ConvertDataTableToList<ProviderCHOPListCHOP>(ds.Tables[11]).ToArray(),
        //        ProviderContact = DatatableHelper.ConvertDataTableToList<ProviderContactListContact>(ds.Tables[12]).ToArray(),
        //        ProviderManagedEmployees = DatatableHelper.ConvertDataTableToList<ProviderManagedEmployeesListManagedEmployee>(ds.Tables[13]).ToArray(),
        //        ProviderOwnership = DatatableHelper.ConvertDataTableToList<UpdateProviderOwnershipList>(ds.Tables[14]).ToArray(),
        //        ProviderProgramAffiliations = DatatableHelper.ConvertDataTableToList<ProgramAffiliationsListProgramAffiliation>(ds.Tables[15]).ToArray(),
        //        ProviderReviews = DatatableHelper.ConvertDataTableToList<ProviderReviewsListReview>(ds.Tables[16]).ToArray(),
        //        ProviderServiceLocation = DatatableHelper.ConvertDataTableToList<ProviderServiceLocation>(ds.Tables[17]).ToArray(),
        //        ProviderServices = DatatableHelper.ConvertDataTableToList<ProviderServicesListServices>(ds.Tables[18]).ToArray(),
        //        ProviderTaxonomyClassification = DatatableHelper.ConvertDataTableToList<TaxonomyClassificationListTaxonomyClassification>(ds.Tables[19]).ToArray(),
        //        ProviderType = DatatableHelper.ConvertDataTableToList<ProviderTypeListType>(ds.Tables[20]).ToArray()
        //    };

        //    string moduleTransactionId;
        //    string responseCode;
        //    string responseType;
        //    string oDSProviderDemographicsId;
        //    string responseMessage;
        //    string responseDetails;
        //    providerManagementService.updateProvider(messageHeader, updateRequestPayload, out moduleTransactionId, out responseCode, out responseType, out oDSProviderDemographicsId, out responseMessage, out responseDetails);
        //    ServiceResponse enrollmentResponse = new ServiceResponse()
        //    {
        //        ModuleTransactionId = moduleTransactionId,
        //        ResponseCode = responseCode,
        //        ODSProviderDemographicsId = oDSProviderDemographicsId,
        //        ResponseDetails = responseDetails,
        //        ResponseMessage = responseMessage,
        //        ResponseType = responseType
        //    };
        //    return enrollmentResponse;
        //}

        //public static InquireProviderInformation[] GetInquireProvider(SearchProvider searchProvider, ProviderManagementMessageHeader messageHeader)
        //{
        //    ProviderManagementService providerManagementService = new ProviderManagementService();
        //    string moduleTransactionId;
        //    string responseCode;
        //    string responseType;
        //    string oDSProviderDemographicsId;
        //    Corp.Core.Libraries.ProviderManagementReference.InqMessageHeader inqMessageHeader;
        //    InquireProviderInformation[] inquireProviderInformation;
        //    providerManagementService.inquireProvider(messageHeader, searchProvider, out moduleTransactionId, out responseCode, out responseType, out oDSProviderDemographicsId, out inqMessageHeader, out inquireProviderInformation);

            //    return inquireProviderInformation;
            //}
            //public static InquireProviderHistoryResponseResponsePayload GetInquireProviderHistory(SearchProviderHistory searchProviderHistory, ProviderManagementMessageHeader messageHeader)
            //{
            //    ProviderManagementService providerManagementService = new ProviderManagementService();
            //    string moduleTransactionId;
            //    string responseCode;
            //    string responseType;
            //    string oDSProviderDemographicsId;
            //    Corp.Core.Libraries.ProviderManagementReference.InqMessageHeader inqMessageHeader;
            //    InquireProviderHistoryResponseResponsePayload inquireProviderInformation;
            //    providerManagementService.inquireProviderHistory(messageHeader, searchProviderHistory, out moduleTransactionId, out responseCode, out responseType, out oDSProviderDemographicsId, out inqMessageHeader, out inquireProviderInformation);

            //    return inquireProviderInformation;
            //}
            public static List<Inquire1099Information> FinancialInquire1099(Search1099 search1099, FinancialMessageHeader messageHeader)
        {
            try
            {

                FinancialServiceService financialService = new FinancialServiceService();
                string ResponseCode;
                Inquire1099ResponseResponseType ResponseType;
                string ResponseMessage;
                // Corp.Core.Libraries.FinancialServiceReference.InqMessageHeader MessageHeader1;
                Inquire1099Information[] Inquire1099ResponsePayload;

                financialService.inquire1099(messageHeader, search1099, out string ModuleTransactionId,
                    out string AdditionalModuleTransactionId, out ResponseCode, out ResponseType,
                    out bool ResponseTypeSpecified, out ResponseMessage, out string ResponseDetails,
                    out Inquire1099ResponsePayload);

                // financialService.inquire1099(messageHeader, search1099, out  ResponseCode, out  ResponseType, out  ResponseMessage, 
                // out string ResponseDetails, out  MessageHeader1, out Inquire1099ResponsePayload);


                return Inquire1099ResponsePayload.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static TransactionHistory FinancialInquireTransactionHistory(SearchTransactionHistory searchTransactionHistory, FinancialMessageHeader messageHeader)
        {
            try
            {
                
                FinancialServiceService financialService = new FinancialServiceService();
                string ResponseCode;
                InquireTransactionHistoryResponseResponseType ResponseType;
                string ResponseMessage;
                // Corp.Core.Libraries.FinancialServiceReference.InqMessageHeader MessageHeader1;
                TransactionHistory transactionHistories;
                financialService.inquireTransactionHistory(messageHeader, searchTransactionHistory,
                    out string ModuleTransactionId, out string AdditionalModuleTransactionId, out ResponseCode,
                    out ResponseType, out bool ResponseTypeSpecified, out ResponseMessage,
                    out string ResponseDetails, out transactionHistories);

                // financialService.inquireTransactionHistory(messageHeader, searchTransactionHistory, out ResponseCode, out ResponseType, out ResponseMessage,
                //  out string ResponseDetails, out MessageHeader1, out transactionHistories);


                return transactionHistories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetInquireClaims()
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                ds.ReadXml(templateActualPath + @"/Cliam_InquireClaimsResponse.xml");
                //ds.ReadXml(@"C:\Tem\Package5\PDMS\PDMS\ProviderDataManagementSystemService\Documents\RecipientEligibilityRequest.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
