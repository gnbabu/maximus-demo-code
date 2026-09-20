using Amazon.S3.Model;
using Amazon.S3;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDMSRestServices.Controllers.Facade;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Reflection;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class AttachmentController : ControllerBase
    {
        [HttpGet]
        [Route("GetPresignUrl")]
        public IActionResult GetPresignUrl(string fileName, string contentType, string method)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(contentType) || string.IsNullOrWhiteSpace(method))
            {
                return BadRequest("Invalid parameters");
            }

            string methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
            Helper.CreateAndReturnLogInfoThreadNumber(methodName + "GetPresignUrl Started.");

            ProcessAttachments processAttachments = new ProcessAttachments();
            DateTime expiryTime = DateTime.Now.AddMinutes(15);
            var preSignedUrl = processAttachments.generatePreSignedUrl(fileName, contentType, expiryTime, method);

            methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
            Helper.CreateAndReturnLogInfoThreadNumber(methodName + $"Extracted presignedUrl and the Url is:{preSignedUrl}");

            var responseJson = new { preSignedUrl };
            return Ok(responseJson);
        }

        [HttpGet("PriorAuthAttachmentCount")]
        public IActionResult GetPriorAuthAttachmentCount([FromQuery] string claimType, [FromQuery] string medicaidId)
        {
            string methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
            Helper.CreateAndReturnLogInfoThreadNumber(methodName + $"PriorAuthAttachmentCount Started for MedicaidId:{medicaidId}");

            int count = AttachmentsFacade.SelectPriorAuthAttachment(claimType, medicaidId);

            methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
            Helper.CreateAndReturnLogInfoThreadNumber(methodName + $"PriorAuthAttachmentCount ended for MedicaidId:{medicaidId} with the count:{count}");

            return Ok(new { attachmentCount = count });
        }

        [HttpPost("SavePAUploadedFileData")]
        public IActionResult SavePAUploadedFile([FromBody] AttachmentUploadRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { Error = "Invalid request payload" });
            }

            string methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
            Helper.CreateAndReturnLogInfoThreadNumber(methodName + $"SavePAUploadedFileData Started for MedicaidId:{request.MedicaidId}");

            try
            {
                if (!HelperFacade.IsValidExtension(request.FileName, out string errMsg))
                {
                    return BadRequest(new { Error = errMsg });
                }

                int documentId = AttachmentsFacade.SavePAUploadedFile(request);

                methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
                Helper.CreateAndReturnLogInfoThreadNumber(methodName + $"SavePAUploadedFileData ended for MedicaidId:{request.MedicaidId} with DocumentId: {documentId}");

                return Ok(new { DocumentId = documentId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "File upload failed", Message = ex.Message });
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                string methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
                Helper.CreateAndReturnLogInfoThreadNumber(methodName + $"DownloadFile Started for File Name:{fileName}");

                ProcessAttachments processAttachments = new ProcessAttachments();
                var fileBytes = await processAttachments.GetAttachmentAsync(fileName);

                methodName = $"SubmitPAAttachments-{MethodInfo.GetCurrentMethod()}";
                Helper.CreateAndReturnLogInfoThreadNumber(methodName + $"DownloadFile ended for File Name:{fileName}");

                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("CheckClaimIsDelet")]
        public IActionResult CheckClaimIsDelet(string claimNumber)
        {
            
            var attachments = AttachmentsFacade.CheckClaimIsDelet(Convert.ToInt32(claimNumber));

            return Ok(attachments);
        }

        [HttpGet]
        [Route("GetAttachmentsByMedicaid")]
        public IActionResult GetAttachmentsByMedicaid(string medicaidId, string claimNumber, string claimType)
        {
            if (string.IsNullOrWhiteSpace(medicaidId))
                return BadRequest("Medicaid ID is required.");

            if (string.IsNullOrWhiteSpace(claimNumber))
                return BadRequest("Claim Number is required.");


            var attachments = AttachmentsFacade.GetAttachmentsByMedicaid(medicaidId, claimNumber, claimType);

            return Ok(attachments);
        }

        [HttpDelete]
        [Route("DeleteAttachments/{documentId}")]
        public IActionResult DeleteAttachment(int documentId)
        {
            if (documentId <= 0)
                return BadRequest("Invalid document ID.");

            AttachmentsFacade.DeleteAttachmentById(documentId);
            return Ok(new { message = "Attachment deleted successfully." });
        }
        [HttpPost]
        [Route("DeleteUnSentAttachment")]
        public IActionResult DeleteUnSentAttachment([FromBody] DocumentStatusUpdateRequest request)
        {
            if (request == null || request.DocumentIds == null || !request.DocumentIds.Any())
                return BadRequest("Document IDs are required.");

            bool success = AttachmentsFacade.UpdateDocumentStatuses(request.DocumentIds);

            AttachmentsFacade.DeleteUnSentAttachment(request.DocumentIds);
            return Ok(new { message = "Attachment deleted successfully." });
        }

        [HttpPost]
        [Route("SaveStandaloneAttachment")]
        public IActionResult SaveStandaloneAttachment([FromBody] AttachmentPayloadModel model)
        {
            if (model == null)
                return BadRequest("Attachment payload is missing.");

            try
            {
                var success = AttachmentsFacade.SaveStandaloneAttachment(model);

                if (success)
                    return Ok(new { message = "Attachment saved successfully." });
                else
                    return StatusCode(500, new { message = "Failed to save attachment." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while saving attachment.", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("InsertClaimsOutBoundDocumentUploads")]
        public IActionResult InsertClaimsOutBoundDocumentUploads([FromBody] AttachmentPayloadModel model)
        {
            if (model == null)
                return BadRequest("Attachment payload is missing.");

            try
            {
                var success = AttachmentsFacade.InsertClaimsOutBoundDocumentUploads(model);

                if (success)
                    return Ok(new { message = "Attachment saved successfully." });
                else
                    return StatusCode(500, new { message = "Failed to save attachment." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occurred while saving attachment.", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("SelectClaimAttachmentTypeId")]
        public IActionResult SelectClaimAttachmentTypeId(string typename, string claim)
        {
            int claimAttachementType = RegistrationFacade.SelectClaimAttachmentTypeId(typename, claim);
            return Ok(claimAttachementType);

        }

        [HttpGet]
        [Route("SelectProviderByGRPMedicaidID")]
        public IActionResult SelectProviderByGRPMedicaidID(string medicaidId)
        {
            try
            {
                string docID = string.Empty;
                DataSet ds = RegistrationFacade.SelectProviderByGRPMedicaidID("3417427");
                DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
                if (Helper.HasRows(dtMisc))
                {
                    DataRow dr = dtMisc.Rows[0];
                    string NPI = Helper.GetString("NPI", dr);
                    long epocdatetime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                    docID = NPI + epocdatetime;

                }
                return Ok(docID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [Route("UpdateDocumentStatuses")]
        public IActionResult UpdateDocumentStatuses([FromBody] DocumentStatusUpdateRequest request)
        {
            try
            {
                if (request == null || request.DocumentIds == null || !request.DocumentIds.Any())
                    return BadRequest("Document IDs are required.");

                bool success = AttachmentsFacade.UpdateDocumentStatuses(request.DocumentIds);

                return Ok(new { message = "Document statuses updated successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Error occurred while UpdateDocumentStatuses.", error = ex.Message });
            }
        }


    }
}
