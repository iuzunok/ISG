using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISGWebSite.Startup))]
namespace ISGWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
