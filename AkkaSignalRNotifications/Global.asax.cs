using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Akka.Actor;
using Akka.Routing;
using AkkaSignalRNotifications.Actors;
using AkkaSignalRNotifications.Messages;

namespace AkkaSignalRNotifications
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static ActorSystem ActorSystem;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ActorSystem = ActorSystem.Create("webcrawler");
            var notificationActor = ActorSystem.ActorOf(Props.Create(() => new NotificationActor()), "notificationActor");
            SystemActors.NotificationActor = notificationActor;
            SystemActors.SignalRActor = ActorSystem.ActorOf(Props.Create(() => new SignalRActor(notificationActor)), "signalRActor");

        }
    }
}
