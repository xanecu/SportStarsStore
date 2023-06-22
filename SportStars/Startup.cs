using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SportStars.Startup))]
namespace SportStars
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
