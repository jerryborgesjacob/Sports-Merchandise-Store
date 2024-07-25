using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sports_Merchandise_Store.Startup))]
namespace Sports_Merchandise_Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
