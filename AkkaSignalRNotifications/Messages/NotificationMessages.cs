using System;
using Akka.Actor;

namespace AkkaSignalRNotifications.Messages
{

    public enum JobExecutionStatus
    {
        BuildingWorkspace,
        Processing,
        Succeeded,
        Failed
    }


    public class ProgressStageSnapshot
    {
        public long Succeeded { get; private set; }
        public long Failed { get; private set; }

        public long Total
        {
            get
            {
                return Succeeded + Failed;
            }
        }

        public ProgressStageSnapshot(long succeeded, long failed)
        {
            Succeeded = succeeded;
            Failed = failed;
        }
    }

    public class ProgressSnapshotMessage
    {
        public JobExecutionStatus Status { get; private set; }
        public ProgressStageSnapshot Consumed { get; private set; }
        public ProgressStageSnapshot Serialised { get; private set; }
        public long TotalEvents { get; private set; }
        public bool IsStillCalculatingTotalEvents { get; private set; }

        public ProgressSnapshotMessage(JobExecutionStatus status, ProgressStageSnapshot consumed, ProgressStageSnapshot serialised, long totalEvents, bool isCalculatingTotalEvents)
        {
            Status = status;
            Consumed = consumed;
            Serialised = serialised;
            TotalEvents = totalEvents;
            IsStillCalculatingTotalEvents = isCalculatingTotalEvents;
        }
    }

    [Flags]
    public enum NotificationTypes
    {
        None = 0,
        Log = 1,
        Progress = 2
    }

    public enum LogLevel
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Fatal,
        None
    }

    public class LogMessage
    {
        public LogLevel Level { get; private set; }
        public string Message { get; private set; }

        public LogMessage(LogLevel level, string message)
        {
            Level = level;
            Message = message;
        }
    }

    public class SubscribeMessage
    {
        public NotificationTypes InterestedIn { get; private set; }
        public LogLevel LogLevel { get; private set; }
        public IActorRef Subscriber { get; private set; }

        public SubscribeMessage(IActorRef subscriber, NotificationTypes interestedIn)
            : this(subscriber, interestedIn, LogLevel.None)
        {

        }

        public SubscribeMessage(IActorRef subscriber, NotificationTypes interestedIn, LogLevel logLevel)
        {
            Subscriber = subscriber;
            InterestedIn = interestedIn;
            LogLevel = logLevel;
        }
    }

    public class UnsubscribeMessage
    {
        public IActorRef Subscriber { get; private set; }

        public UnsubscribeMessage(IActorRef subscriber)
        {
            Subscriber = subscriber;
        }
    }
}