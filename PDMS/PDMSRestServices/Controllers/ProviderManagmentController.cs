using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;
using System.Data;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class ProviderManagmentController : ControllerBase
    {
        [HttpGet]
        [Route("ProviderManagementEnrollRequest")]
        public IActionResult ProviderManagementEnrollRequest(int transactionID, bool makeRequest = false)
        {
            string txnResult = string.Empty;
            ProviderManagmentFacade pm = new ProviderManagmentFacade();
            txnResult = pm.providerManagementEnrollRequest(transactionID, makeRequest);
            return Ok(txnResult);
        }

        [HttpGet]
        [Route("ProviderManagementUpdateRequest")]
        public IActionResult ProviderManagementUpdateRequest(int transactionID, bool makeRequest = false)
        {
            string txnResult = string.Empty;
            ProviderManagmentFacade pm = new ProviderManagmentFacade();
            txnResult = pm.providerManagementUpdateRequest(transactionID, makeRequest);
            return Ok(txnResult);
        }
    }
}
