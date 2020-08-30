using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PathToMastery.Models;
using PathToMastery.Services.Abstract;

namespace PathToMastery.Services
{
    public class NotificationService : IHostedService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IDbService _dbService;
        private readonly ConcurrencyService _concurrencyService;
        private readonly PathService _pathService;
        private readonly ISocialService _socialService;
        private Timer _timer;

        public NotificationService(
            ILogger<NotificationService> logger,
            IDbService dbService,
            ConcurrencyService concurrencyService,
            PathService pathService,
            ISocialService socialService
        )
        {
            _logger = logger;
            _dbService = dbService;
            _concurrencyService = concurrencyService;
            _pathService = pathService;
            _socialService = socialService;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoBackgroundJob, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            _logger.LogInformation("Background check started");
            return Task.CompletedTask;
        }

        private void DoBackgroundJob(object state = null)
        {
            CheckUsers();
            _concurrencyService.CleanOld();
        }
        
        private void CheckUsers()
        {
            var now = Utils.Now();
            var users = _dbService.Collection<User>()
                .Where(u => u.NotifyTime > 0 && u.NotifyTime < now)
                .ToList();
            
            foreach (var user in users)
            {
                if (user.NotifyPathId <= 0) continue;
                var path = _pathService.PathFromId(user, user.NotifyPathId);
                var message = $"Время совершить шаг по пути: \"{path.Name}\"";
                
                // notify user
                _socialService.Notify(new []{user.Id}, message);
                
                // clear notify time
                user.NotifyTime = 0;
                user.NotifyPathId = 0;
                _dbService.Update(user);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _logger.LogInformation("Background check stopped");
            return Task.CompletedTask;
        }
    }
}