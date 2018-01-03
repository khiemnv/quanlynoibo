using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(testWFS.Startup))]
namespace testWFS
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
