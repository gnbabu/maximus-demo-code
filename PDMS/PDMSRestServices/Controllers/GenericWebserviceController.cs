using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class GenericWebserviceController : Controller
    {
        [HttpPost]
        [Route("GenericSIWSRequest")]
        public IActionResult GenericSIWSRequest([FromBody] GenericWSModel model)
        {
            string txnResult = string.Empty;
            Models.ResponseDetail response = new Models.ResponseDetail();
            GenericWSFacade gws = new GenericWSFacade();
            response = gws.makeGenericSIWSRequest(model.wsUrl, model.hostName, model.soapAction, model.username, model.password, model.certificateName, model.requestWS);
            return Ok(response);
        }
    }
}
