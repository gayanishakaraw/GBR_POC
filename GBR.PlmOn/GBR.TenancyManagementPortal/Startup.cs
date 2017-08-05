using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gerber.TenancyManagementPortal.Startup))]
namespace Gerber.TenancyManagementPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
