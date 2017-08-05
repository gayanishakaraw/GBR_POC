using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GBR.PlmOn.Startup))]
namespace GBR.PlmOn
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
