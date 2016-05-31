using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EbaySalesTracker.Startup))]
namespace EbaySalesTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
