using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.ServiceModel;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Facade;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class ClaimsRegistrationController : ControllerBase
    {
        [HttpGet]
        [Route("SelectClaimAttachmentTypeId")]
        public IActionResult SelectClaimAttachmentTypeId(string typename, string claim)
        {
            int claimAttachementType = RegistrationFacade.SelectClaimAttachmentTypeId(typename, claim);
            return Ok(claimAttachementType);

        }

        //[HttpPost]
        //[Route("GenerateDocumentNumber")]
        //public IActionResult GenerateDocumentNumber(string MedicaidId)
        //{
        //    string providerNPI = string.Empty;
        //    DataSet ds = RegistrationFacade.GenerateDocumentNumber(MedicaidId);
        //    DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        //    Guid guidnpi = Guid.NewGuid();
        //    if (Helper.HasRows(dtMisc))
        //    {
        //        DataRow dr = dtMisc.Rows[0];
        //        providerNPI = Helper.GetString("NPI", dr);
        //    }

        //    return Ok(providerNPI + " " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")); //guidnpi;

        //}

        [HttpGet]
        [Route("SelectProviderByGRPMedicaidID")]
        public IActionResult SelectProviderByGRPMedicaidID(string MedicaidId)
        {
            try
            {
                string docID = string.Empty;
                DataSet ds = RegistrationFacade.SelectProviderByGRPMedicaidID(MedicaidId);
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
    }
}
