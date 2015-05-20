using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GetWiki.Startup))]
namespace GetWiki
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
