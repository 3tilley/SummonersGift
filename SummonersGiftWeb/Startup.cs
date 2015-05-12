using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SummonersGift.Web.Startup))]
namespace SummonersGift.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
