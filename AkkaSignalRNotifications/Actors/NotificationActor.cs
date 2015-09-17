using System.Collections.Generic;
using Akka.Actor;
using AkkaSignalRNotifications.Messages;

namespace AkkaSignalRNotifications.Actors
{

    public class NotificationActor : ReceiveActor
    {
        private readonly HashSet<LogSubscriber> _logSubscribers = new HashSet<LogSubscriber>();
        private readonly HashSet<IActorRef> _progressSubscribers = new HashSet<IActorRef>();

        public HashSet<LogSubscriber> LogSubscribers
        {
            get
            {
                return _logSubscribers;
            }
        }

        public HashSet<IActorRef> ProgressSubscribers
        {
            get
            {
                return _progressSubscribers;
            }
        }

        public NotificationActor()
        {
            Receive<SubscribeMessage>(m =>
            {
                if (m.InterestedIn.HasFlag(NotificationTypes.Log))
                {
                    _logSubscribers.Add(new LogSubscriber(m.Subscriber, m.LogLevel));
                }

                if (m.InterestedIn.HasFlag(NotificationTypes.Progress))
                {
                    _progressSubscribers.Add(m.Subscriber);
                }
            });

            Receive<UnsubscribeMessage>(m =>
            {
                _logSubscribers.RemoveWhere(x => x.Subscriber.Equals(m.Subscriber));
                _progressSubscribers.Remove(m.Subscriber);
            });

            Receive<ProgressSnapshotMessage>(m =>
            {
                foreach (IActorRef sub in _progressSubscribers)
                {
                    sub.Tell(m);
                }
            });

            Receive<LogMessage>(m =>
            {
                foreach (LogSubscriber sub in _logSubscribers)
                {
                    if (m.Level >= sub.InterestedLevel)
                    {
                        sub.Subscriber.Tell(m);
                    }
                }
            });
        }

        public class LogSubscriber : IEqualityComparer<LogSubscriber>
        {
            public LogLevel InterestedLevel { get; private set; }
            public IActorRef Subscriber { get; private set; }

            public LogSubscriber(IActorRef subscriber, LogLevel interestedLevel)
            {
                Subscriber = subscriber;
                InterestedLevel = interestedLevel;
            }

            public bool Equals(LogSubscriber x, LogSubscriber y)
            {
                return x.Subscriber.Equals(y.Subscriber);
            }

            public int GetHashCode(LogSubscriber x)
            {
                return x.Subscriber.GetHashCode();
            }
        }
    }
}