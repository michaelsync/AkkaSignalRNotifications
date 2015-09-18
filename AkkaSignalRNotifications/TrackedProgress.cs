using AkkaSignalRNotifications.Messages;

namespace AkkaSignalRNotifications
{
    public class TrackedProgress
    {
        public JobExecutionStatus Status { get; set; }

        public StageProgress Consumed { get; set; }

        public StageProgress Serialised { get; set; }

        public long TotalEvents { get; set; }

        public bool IsStillCalculatingTotalEvents { get; set; }

        public TrackedProgress()
        {
            IsStillCalculatingTotalEvents = true;
            Consumed = new StageProgress();
            Serialised = new StageProgress();
        }

        internal ProgressSnapshotMessage ToMessage()
        {
            return new ProgressSnapshotMessage(Status, new ProgressStageSnapshot(Consumed.Succeeded, Consumed.Failed), new ProgressStageSnapshot(Serialised.Succeeded, Serialised.Failed),
                TotalEvents, IsStillCalculatingTotalEvents);
        }
    }

    public class StageProgress
    {
        public long Succeeded { get; set; }

        public long Failed { get; set; }

        public long Total => Succeeded + Failed;
    }

}