using System.Web.Http;

namespace Api
{
    partial class Startup
    {
        private HttpConfiguration GetWebApiConfiguration()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            return config;
        }
    }
}
