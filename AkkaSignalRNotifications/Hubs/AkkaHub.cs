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
    }
}