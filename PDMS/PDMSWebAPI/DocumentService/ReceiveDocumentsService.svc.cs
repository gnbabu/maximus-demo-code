using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using PDMSWebAPI.DocumentService;
using System.Data;
using System.Reflection;
using System.IO;
using MAXIMUS.Core.Libraries;
//using MAXIMUS.Core.Libraries.Appsettings;
using PDMSWebAPI.Models;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReceiveDocumentsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReceiveDocumentsService.svc or ReceiveDocumentsService.svc.cs at the Solution Explorer and start debugging.
    
    public class ReceiveDocumentsService : IReceiveDocumentsService
    {
       
        public ReceiveDocumentResponse ReceiveAttachmentDocuments(ReceiveDocumentsRequest request)
        {
            ReceiveDocumentResponse robj= new ReceiveDocumentResponse();
            List<SendAttachmentResponse> objSendRes = new List<SendAttachmentResponse>();
          


            if (request!=null)
            {

                //Vlidate user todo

                //Validate Role todo

                //size validation need to do.

                robj.StateCode = request.StateCode;
                robj.SITransactionKey = request.SITransactionKey;
                robj.RequestTimeStamp = request.RequestTimeStamp;
                robj.ModuleTransactionId = request.ModuleTransactionId;
                robj.RequestorSystem = request.RequestorSystem;
                robj.AdditionalModuleTransactionId = request.AdditionalModuleTransactionId;
                string requestFileName = request.SITransactionKey + "_" + "ReceiveDocumentsService" + "_" + request.RequestTimeStamp.ToString();
                string templateActualPath = @"c:\testdata\";
                string documnetsServiceFolder = @"ReceiveDocumentsService\";

                string totalFilePath = templateActualPath + documnetsServiceFolder;

                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(request.GetType());
              
                    StreamWriter sw = new StreamWriter(templateActualPath + documnetsServiceFolder+ requestFileName);
                    xs.Serialize(sw, request);

                PDMSWebAPI.DocumentService.DocumentService obj = new PDMSWebAPI.DocumentService.DocumentService();


                //save The file Contents to Server.
                SaveBinaryContentToFiles(request, totalFilePath);


                SendAttachmentResponse[] res= obj.UploadDocumentToDB(request, requestFileName);

            }
            else
            {

                robj.StateCode = string.Empty;
                robj.SITransactionKey = string.Empty;
                robj.RequestTimeStamp = DateTime.Now;
                robj.ModuleTransactionId = string.Empty;
                robj.RequestorSystem = string.Empty;

                robj.AdditionalModuleTransactionId = string.Empty; 

                SendAttachmentResponse saResponse = new SendAttachmentResponse();
                saResponse.IndexID = "";
                saResponse.ResponseType = "Error";
                saResponse.ResponseCode = "E08";
                saResponse.ResponseMessage = DocumentErrorCodes.E08.ToString();

                objSendRes.Add(saResponse);
            }


            robj.AttachmentResponses = objSendRes.ToArray();
            return robj;
        }

        private void SaveBinaryContentToFiles(ReceiveDocumentsRequest request, string totalFilePath)
        {
            try
            {

                foreach(SendAttachment sa in request.Attachments)
                {
                    if(sa!=null)
                    {

                        string fileName = sa.DocumentName;
                        string fileExtension = sa.DocumentExtension;
                        string fileNameFinal = totalFilePath+fileName + fileExtension;

                        byte[] fileBytes = (sa.Attachment64BitBinary);

                        using (var fs = new FileStream(fileNameFinal, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(fileBytes, 0, fileBytes.Length);
                            
                        }


                    }


                }


            }
            catch(Exception ex)
            {

            }
        }
    }
}
