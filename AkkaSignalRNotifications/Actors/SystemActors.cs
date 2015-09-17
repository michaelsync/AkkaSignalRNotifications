using Akka.Actor;

namespace AkkaSignalRNotifications.Actors
{
    public class SystemActors
    {
        public static IActorRef NotificationActor = ActorRefs.Nobody;
        public static IActorRef SignalRActor = ActorRefs.Nobody; 
    }
}