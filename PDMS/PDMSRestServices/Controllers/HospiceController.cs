using MAXIMUS.Controllers.PDMS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class HospiceController : ControllerBase
    {
        [HttpGet]
        [Route("GetCountyData")]
        public IActionResult GetCountyData(string state = "")
        {
            var data = string.Empty;
            var ds = LookupTableController.SelectCounty(state);
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
                return Ok(data);
            }
            return Ok(JsonConvert.SerializeObject(data));
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string npi = "", string medicaidid = "", string lastName = "", string firstName = "")
        {
            var data = "";
            var ds = ProviderController.SearchProviderNPI(npi, medicaidid, lastName, firstName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
                return Ok(data);
            }
            return Ok(JsonConvert.SerializeObject(data));
        }
    }
}
