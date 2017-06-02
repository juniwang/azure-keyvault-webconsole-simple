using Microsoft.Owin;
using Owin;
using KeyVaultManagement.Middlewares;

[assembly: OwinStartupAttribute(typeof(KeyVaultManagement.Startup))]
namespace KeyVaultManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(ConfigMiddleware.GetConfig);
        }
    }
}
