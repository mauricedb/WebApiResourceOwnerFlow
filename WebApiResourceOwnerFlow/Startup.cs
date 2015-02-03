using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace WebApiResourceOwnerFlow
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var provider = new OAuthAuthorizationServerProvider()
            {
                OnValidateClientAuthentication = ctx =>
                {
                    ctx.Validated();
                    return Task.FromResult(0);
                },
                OnGrantResourceOwnerCredentials = ctx =>
                {
                    if (ctx.UserName == "maurice" && ctx.Password == "pass")
                    {
                        var identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, ctx.UserName)
                        }, "password");
                        ctx.Validated(identity);
                    }
                    return Task.FromResult(0);
                }
            };

            var options = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = provider,
                AllowInsecureHttp = true // Never do this in production
            };
            app.UseOAuthAuthorizationServer(options);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                AuthenticationType = "Bearer"
            });


            app.Map("/api", apiApp =>
            {
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                config.SuppressDefaultHostAuthentication();
                config.Filters.Add(new HostAuthenticationAttribute("Bearer"));
                apiApp.UseWebApi(config);
            });
        }
    }
}