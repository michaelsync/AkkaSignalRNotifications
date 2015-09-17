using Akka.Actor;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AkkaSignalRNotifications.Actors
{
    public class SignalRActor : ReceiveActor
    {
        private AkkaHub _hub;

        public SignalRActor()
        {
            
        }

        protected override void PreStart()
        {
            var hubManager = new DefaultHubManager(GlobalHost.DependencyResolver);
            _hub = hubManager.ResolveHub("akkaHub") as AkkaHub;
        }


    }
}