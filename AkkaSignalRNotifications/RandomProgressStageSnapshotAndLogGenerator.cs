using System;
using System.Linq;
using System.Threading;
using Akka.Actor;
using AkkaSignalRNotifications.Actors;
using AkkaSignalRNotifications.Messages;
using Microsoft.Ajax.Utilities;

namespace AkkaSignalRNotifications
{
    public static class RandomProgressStageSnapshotAndLogGenerator
    {
        private static bool _isOn;

        public static void Start()
        {
            _isOn = true;

            Random rnd = new Random((int)DateTime.Now.Ticks);
            Random longRandom = new Random((int)DateTime.Now.Ticks);

            while (_isOn)
            {
                SendMessage(rnd, longRandom);

                Thread.Sleep(1500);
            }
        }

        private static void SendMessage(Random rnd, Random longRandom)
        {
            var randomValue = rnd.Next(1);
            if (randomValue == 0)
            {
                SystemActors.NotificationActor.Tell(CreateProgressSnapshotMessage(longRandom));
            }
            else
            {
                SystemActors.NotificationActor.Tell(new LogMessage(RandomEnumValue<LogLevel>(), GenerateRandomString()));
            }
        }


        private static ProgressSnapshotMessage CreateProgressSnapshotMessage(Random longRandom)
        {
            return new ProgressSnapshotMessage(RandomEnumValue<JobExecutionStatus>(),
                new ProgressStageSnapshot(Convert.ToInt64(longRandom.Next(30000)),
                    Convert.ToInt64(longRandom.Next(30000))),
                new ProgressStageSnapshot(Convert.ToInt64(longRandom.Next(30000)),
                    Convert.ToInt64(longRandom.Next(30000))),
                Convert.ToInt64(longRandom.Next(30000)),
                false
                );
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