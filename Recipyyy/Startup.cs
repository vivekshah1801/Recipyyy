using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Recipyyy.Startup))]
namespace Recipyyy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
