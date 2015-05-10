using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SummonersGiftWeb.Startup))]
namespace SummonersGiftWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
