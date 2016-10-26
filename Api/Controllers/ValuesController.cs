using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/values")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}
