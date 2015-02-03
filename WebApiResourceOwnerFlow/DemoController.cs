using System.Security.Claims;
using System.Web.Http;

namespace WebApiResourceOwnerFlow
{
    [Route("demo")]
    [Authorize]
    public class DemoController : ApiController
    {
        public string Get()
        {
            var name = "anonymous";

            var nameClaim = ((ClaimsPrincipal)User).FindFirst(ClaimTypes.Name);
            if (nameClaim != null)
            {
                name = nameClaim.Value;
            }

            return "The user is: " + name;
        }
    }
}