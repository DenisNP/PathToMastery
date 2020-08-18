using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PathToMastery.Services
{
    public class ConcurrencyService
    {
        private readonly ConcurrentDictionary<string, DateTime> _lastRequests = new ConcurrentDictionary<string, DateTime>();

        public void CleanOld()
        {
            var minTime = DateTime.Now - new TimeSpan(0, 1, 0);
            var removeIds = _lastRequests
                .Where(r => r.Value < minTime)
                .Select(r => r.Key).ToList();
            
            foreach (var removeId in removeIds)
            {
                _lastRequests.TryRemove(removeId, out _);
            }
        }

        public bool CheckAddRequest(string userId)
        {
            var now = DateTime.Now;
            if (_lastRequests.TryGetValue(userId, out var time))
            {
                if (time < now - new TimeSpan(0, 0, 2))
                {
                    _lastRequests.TryUpdate(userId, now, time);
                    return true;
                }

                return false;
            }

            _lastRequests.TryAdd(userId, now);
            return true;
        }
    }
}