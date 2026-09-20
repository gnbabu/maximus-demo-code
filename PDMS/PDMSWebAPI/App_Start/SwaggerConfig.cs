using System.Web.Http;
using WebActivatorEx;
using PDMSWebAPI;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace PDMSWebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "PDMSWebAPI");
                    })
                .EnableSwaggerUi(c =>{ });
        }
    }
}
