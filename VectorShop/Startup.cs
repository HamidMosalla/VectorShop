using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VectorShop.Startup))]
namespace VectorShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
