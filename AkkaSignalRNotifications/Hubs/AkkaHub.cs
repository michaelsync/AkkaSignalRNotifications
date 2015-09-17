using Akka.Actor;
using AkkaSignalRNotifications.Actors;
using AkkaSignalRNotifications.Messages;
using Microsoft.AspNet.SignalR;

namespace AkkaSignalRNotifications.Hubs
{
    public class AkkaHub : Hub
    {
        public void PushProgressSnapshotMessage(ProgressSnapshotMessage update)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<AkkaHub>();
            context.Clients.All.pushProgressStageSnapshot(update);
        }

        public void PushLogMessage(LogMessage update)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<AkkaHub>();
            context.Clients.All.pushLogMessage(update);
        }

        public void Subscribe()
        {
            SystemActors.SignalRActor.Tell(new SubscribeAllMessage());
            RandomProgressStageSnapshotAndLogGenerator.Start();
        }

        public void Unsubscribe()
        {
            RandomProgressStageSnapshotAndLogGenerator.Stop();
            SystemActors.SignalRActor.Tell(new UnsubscribeAllMessage());
        }
    }

    
}