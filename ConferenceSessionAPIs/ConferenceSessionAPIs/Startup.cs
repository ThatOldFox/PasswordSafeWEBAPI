using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ConferenceSessionAPIs.Startup))]
namespace ConferenceSessionAPIs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
