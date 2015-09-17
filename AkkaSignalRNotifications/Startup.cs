using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AkkaSignalRNotifications.Startup))]
namespace AkkaSignalRNotifications
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
