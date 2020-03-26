using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BD_CDMS.Startup))]
namespace BD_CDMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
