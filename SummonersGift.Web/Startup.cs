using Microsoft.Owin;
using Owin;
using Hangfire;

[assembly: OwinStartupAttribute(typeof(SummonersGift.Web.Startup))]
namespace SummonersGift.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Server=(localdb)\\v11.0;Integrated Security=true;");
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            ConfigureAuth(app);
        }
    }
}
