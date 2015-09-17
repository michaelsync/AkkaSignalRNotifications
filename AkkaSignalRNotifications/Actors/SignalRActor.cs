using Akka.Actor;
using AkkaSignalRNotifications.Hubs;
using AkkaSignalRNotifications.Messages;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AkkaSignalRNotifications.Actors
{
    public class SignalRActor : ReceiveActor
    {
        // ReSharper disable once NotAccessedField.Local
        private AkkaHub _hub;
        private readonly IActorRef _notificationActor;

        public SignalRActor(IActorRef notificationActor)
        {
            _notificationActor = notificationActor;

            Receive<SubscribeAllMessage>(m =>
            {
                _notificationActor.Tell(new SubscribeMessage(Self, NotificationTypes.Log, LogLevel.Verbose));
                _notificationActor.Tell(new SubscribeMessage(Self, NotificationTypes.Progress, LogLevel.Verbose));
            });

            Receive<ProgressSnapshotMessage>(m =>
            {
               _hub.PushProgressSnapshotMessage(m);
            });

            Receive<LogMessage>(m =>
            {
                _hub.PushLogMessage(m);
            });
        }

        protected override void PreStart()
        {
            var hubManager = new DefaultHubManager(GlobalHost.DependencyResolver);
            _hub = hubManager.ResolveHub("akkaHub") as AkkaHub;
        }
    }
}