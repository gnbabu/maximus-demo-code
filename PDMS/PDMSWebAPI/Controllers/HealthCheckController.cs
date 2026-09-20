using System.Web.Mvc;

namespace PDMSWebAPI.Controllers
{
    public class HealthCheckController : Controller
    {
        // GET: HealthCheck
        [HttpGet]
        public JsonResult Index()
        {
            string RS = "OK";
            return Json(RS, JsonRequestBehavior.AllowGet);
        }
    }
}