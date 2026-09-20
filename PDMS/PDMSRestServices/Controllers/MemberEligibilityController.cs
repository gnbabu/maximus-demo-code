using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;
using System.Globalization;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class MemberEligibilityController : ControllerBase
    {
        [HttpPost]
        [Route("GetRecipientPageInformationV2")]
        public IActionResult GetRecipientPageInformationV2([FromBody] MemberEligibilityPageRequest meRequest)
        {
            Models.RecipientEligibilitySearchResponseV2 recipientInformation = null;
            try
            {
                string MEtdosStr = meRequest.MEtdos?.ToUniversalTime().ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) ?? "";
                string MEfdosStr = meRequest.MEfdos?.ToUniversalTime().ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) ?? "";
                Guid userId = HelperFacade.GetUserId(meRequest.MEun);
                Models.RecipientEligibilitySearchResponseV2 recipientInfo = null;
                recipientInfo = new EligibilityFacade().SearchRequestV2(meRequest.MEpid, userId, meRequest.MEssn, meRequest.MEdob, MEfdosStr, MEtdosStr,meRequest.MEpc, meRequest.MEmbn, meRequest.MErt);
                return Ok(recipientInfo);
            }
            catch (Exception ex)
            {
                recipientInformation = new Models.RecipientEligibilitySearchResponseV2();
                recipientInformation.ErrorDetails = new List<Models.ErrorDetail>()
                {
                    new Models.ErrorDetail()
                    {
                        Code="Exception",
                        Description="Error: An error occurred while processing the request "
                    }
                };
            }

            return Ok(recipientInformation);
        }
    }
}
