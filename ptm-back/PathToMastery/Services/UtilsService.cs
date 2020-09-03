using System;
using System.Linq;
using PathToMastery.Models;
using PathToMastery.Models.State;

namespace PathToMastery.Services
{
    public class UtilsService
    {
        public DateTimeOffset GetNextCheckpointFor(PathData data, DateTimeOffset date, int dow, bool forceStrictLater = false)
        {
            var nextDays = data.Days.SkipWhile(x => x < dow).ToList();
            var nextDay = nextDays.Count > 0 ? nextDays.First() : data.Days.First();
            return ToClosestUp(date, nextDay, forceStrictLater);
        }

        public DateTimeOffset ToClosestUp(DateTimeOffset d, int dayOfWeek, bool forceStrictLater = false)
        {
            var currentDow = d.NormalDow();

            var diff = dayOfWeek - currentDow;
            if (diff < 0 || diff == 0 && forceStrictLater) diff += 7;

            return d + TimeSpan.FromDays(diff);
        }

        public DateTimeOffset ToClosestDown(DateTimeOffset d, int dayOfWeek)
        {
            var currentDow = d.NormalDow();

            var diff = dayOfWeek - currentDow;
            if (diff > 0) diff -= 7;

            return d + TimeSpan.FromDays(diff);
        }
        
        public (int h, int m) UnpackTime(int time)
        {
            var h = (int)Math.Floor(time * 1.0 / 100.0);
            if (h < 0 || h > 23) throw new ArgumentException($"Invalid hours: {time} > {h}");

            var m = time - h * 100;
            if (m < 0 || m > 59) throw new ArgumentException($"Invalid minutes {time} > {m}");

            return (h, m);
        }
        
        public bool SetNextNotify(User user, int offset)
        {
            var paths = new[] {user.First, user.Second, user.Third};
            
            PathData best = null;
            long earliest = long.MaxValue;
            foreach (var pathData in paths)
            {
                var notifyTime = NextNotify(pathData, offset);
                if (notifyTime > 0 && notifyTime < earliest)
                {
                    earliest = notifyTime;
                    best = pathData;
                }
            }

            if (earliest == long.MaxValue) earliest = 0;
            if (user.NotifyTime == earliest) return false;
            
            user.NotifyTime = earliest;
            user.NotifyPathId = best != null ? Array.IndexOf(paths, best) + 1 : 0;
            
            return true;
        }

        private long NextNotify(PathData data, int offset)
        {
            if (data.Notify < 0 || string.IsNullOrEmpty(data.Name)) return 0;
            var (h, m) = UnpackTime(data.Notify);
            var now = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(offset));
            var dow = now.NormalDow();

            DateTimeOffset notifyDate;
            var notifyTime = new TimeSpan(h, m, 0);
            if (data.Days.Contains(dow) && notifyTime > now.TimeOfDay)
            {
                // notify today but later
                notifyDate = now - now.TimeOfDay + notifyTime;
            }
            else
            {
                // notify on next checkpoint day
                var nextCheckpoint = GetNextCheckpointFor(data, now, dow, true);
                notifyDate = nextCheckpoint - nextCheckpoint.TimeOfDay + notifyTime;
            }

            return notifyDate.ToUnixTimeMilliseconds();
        }
    }
}