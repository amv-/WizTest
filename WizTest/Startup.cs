using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WizTest.Startup))]
namespace WizTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
