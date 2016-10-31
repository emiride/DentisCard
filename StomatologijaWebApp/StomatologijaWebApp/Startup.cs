using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StomatologijaWebApp.Startup))]
namespace StomatologijaWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
