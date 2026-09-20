using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class TransactionDashboardController : ControllerBase
    {
        [HttpGet("transactions")]

        public async Task<IActionResult> GetTransactions([FromQuery] string serviceType, [FromQuery] string operation, [FromQuery] string timeframe)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(serviceType) && string.IsNullOrWhiteSpace(operation) && string.IsNullOrWhiteSpace(timeframe))
                {
                    return BadRequest("At least one query parameter must be provided.");
                }

                var transactions = PDMSRestServices.Facade.TransactionDashboardFacade
                    .GetTransactionMonitoringData(serviceType, operation, timeframe);

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
