using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DryStream.Startup))]
namespace DryStream
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
