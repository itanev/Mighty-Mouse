using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eDoc.Web.Startup))]
namespace eDoc.Web
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
