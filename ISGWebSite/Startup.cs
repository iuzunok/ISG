using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISGWebSite.Startup))]
namespace ISGWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // http://bitoftech.net/2014/10/27/json-web-token-asp-net-web-api-2-jwt-owin-authorization-server/
            //HttpConfiguration config = new HttpConfiguration();

            //// Web API routes
            //config.MapHttpAttributeRoutes();

            //ConfigureOAuth(app);

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //app.UseWebApi(config);

        }
        //public void ConfigureOAuth(IAppBuilder app)
        //{
        //    OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
        //    {
        //        //For Dev enviroment only (on production should be AllowInsecureHttp = false)
        //        AllowInsecureHttp = true,
        //        TokenEndpointPath = new PathString("/oauth2/token"),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
        //        Provider = new CustomOAuthProvider(),
        //        AccessTokenFormat = new CustomJwtFormat("http://jwtauthzsrv.azurewebsites.net")
        //    };

        //    // OAuth 2.0 Bearer Access Token Generation
        //    app.UseOAuthAuthorizationServer(OAuthServerOptions);
        //}



    }
}
