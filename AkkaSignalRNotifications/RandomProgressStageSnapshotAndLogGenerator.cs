using System;
using System.Linq;
using System.Threading;
using Akka.Actor;
using AkkaSignalRNotifications.Actors;
using AkkaSignalRNotifications.Messages;

namespace AkkaSignalRNotifications
{
    public static class RandomProgressStageSnapshotAndLogGenerator
    {
        private static TrackedProgress _progress = new TrackedProgress();
        private static readonly long TotalBusinessEvents = new Random((int)DateTime.Now.Ticks).Next(10000, 1000000);
        private static readonly Random ProgressRandom = new Random((int)DateTime.Now.Ticks);

        private static bool _isOn;

        public static void Start()
        {
            //Random rnd = new Random((int)DateTime.Now.Ticks);
            //Random longRandom = new Random((int)DateTime.Now.Ticks);
            _isOn = true;

            _progress = new TrackedProgress();

            while (_isOn && _progress.Status != JobExecutionStatus.Succeeded)
            {
                SendMessages();

                Thread.Sleep(1000);
            }
        }

        private static void SendMessages()
        {
            UpdateProgress();
            SystemActors.NotificationActor.Tell(new LogMessage(RandomEnumValue<LogLevel>(), GenerateRandomString()));
            SystemActors.NotificationActor.Tell(_progress.ToMessage());
        }

        private static void UpdateProgress()
        {
            // Ugliest code ever.

            switch (_progress.Status)
            {
                case JobExecutionStatus.BuildingWorkspace:
                    ProgressWorkspace();
                    break;
                case JobExecutionStatus.Processing:
                    ProgressProcessing();
                    break;
                default:
                    throw new InvalidOperationException("Should not hit this");
            }
        }

        private static void ProgressProcessing()
        {
            const int chunkSize = 25000;

            if (_progress.Consumed.Total < TotalBusinessEvents)
            {
                long totalSucceeded;

                if (_progress.Consumed.Total + chunkSize < TotalBusinessEvents)
                {
                    int failed = ProgressRandom.Next(0, 10);
                    _progress.Consumed.Failed += failed;

                    totalSucceeded = chunkSize - failed;

                    SystemActors.NotificationActor.Tell(new LogMessage(LogLevel.Information,
                        $"Consumed {chunkSize} business events"));
                }
                else
                {
                    totalSucceeded = TotalBusinessEvents - _progress.Consumed.Total;
                    SystemActors.NotificationActor.Tell(new LogMessage(LogLevel.Information,
                        $"Consumed {totalSucceeded} business events"));
                }

                _progress.Consumed.Succeeded += totalSucceeded;

                _progress.Serialised.Succeeded += ProgressRandom.Next(0, (int)totalSucceeded / 10);
            }

            if (_progress.Serialised.Total < TotalBusinessEvents)
            {
                long totalSucceeded;

                if (_progress.Serialised.Total + chunkSize < TotalBusinessEvents)
                {
                    int failed = ProgressRandom.Next(0, 20);
                    _progress.Serialised.Failed += failed;
                    totalSucceeded = chunkSize - failed;
                    _progress.Serialised.Succeeded += totalSucceeded;

                    SystemActors.NotificationActor.Tell(new LogMessage(LogLevel.Information,
                        $"Serialised {chunkSize} business events"));
                }
                else
                {
                    totalSucceeded = TotalBusinessEvents - _progress.Serialised.Total;
                    _progress.Serialised.Succeeded += totalSucceeded;

                    SystemActors.NotificationActor.Tell(new LogMessage(LogLevel.Information,
                        $"Serialised {totalSucceeded} business events"));
                }
            }

            if (_progress.Serialised.Total == TotalBusinessEvents)
            {
                _progress.Status = JobExecutionStatus.Succeeded;
                SystemActors.NotificationActor.Tell(new LogMessage(LogLevel.Information, "Job execution completed"));
            }
        }


        private static void ProgressWorkspace()
        {
            const int workspaceChunkSize = 100000;

            if (_progress.TotalEvents > 0)
            {
                _progress.Consumed.Succeeded += ProgressRandom.Next(0, workspaceChunkSize / 4);
                _progress.Consumed.Failed += ProgressRandom.Next(0, 10);
            }

            if (_progress.TotalEvents + workspaceChunkSize < TotalBusinessEvents)
            {
                _progress.TotalEvents += workspaceChunkSize;
            }
            else
            {
                _progress.TotalEvents = TotalBusinessEvents;
                _progress.IsStillCalculatingTotalEvents = false;
                _progress.Status = JobExecutionStatus.Processing;
                SystemActors.NotificationActor.Tell(new LogMessage(LogLevel.Information,
                    $"Workspace built with {TotalBusinessEvents} business events to process"));
            }
        }

        private static T RandomEnumValue<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            Random random = new Random((int)DateTime.Now.Ticks);
            return (T)values.GetValue(random.Next(values.Length));
        }

        private static string GenerateRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random((int)DateTime.Now.Ticks);
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
        public static void Stop()
        {
            _isOn = false;
        }
    }
}