using MAXIMUS.Controllers.PDMS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Facade;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class PartialProviderManagmentController : ControllerBase
    {
        [HttpGet]
        [Route("PartialProviderManagementSubmitRequest")]
        public IActionResult PartialProviderManagementSubmitRequest(int transactionID, string sendTo, bool makeRequest = false, bool? ReqRes = true)
        {
            string txnResult = string.Empty;
            PartialProviderManagmentFacade ppm = new PartialProviderManagmentFacade();
            txnResult = ppm.partialProviderManagementSubmitRequest(transactionID, sendTo, makeRequest, ReqRes);
            return Ok(txnResult);
        }

        //[HttpGet]
        //[Route("DoesStateAbbrevExist")]
        //public IActionResult DoesStateAbbrevExist(string stateAbbrev)
        //{
        //    bool isExist = LookupTableController.DoesStateAbbrevExist(stateAbbrev);
        //    return Ok(isExist);
        //}
    }
}
