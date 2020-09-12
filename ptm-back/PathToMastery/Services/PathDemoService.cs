using System;
using PathToMastery.Models.State;
using PathToMastery.Services.Abstract;

namespace PathToMastery.Services
{
    public class PathDemoService : PathService
    {
        private DateTimeOffset _now;
        
        public PathDemoService(
            IDbService dbService,
            UtilsService utilsService,
            MilestoneService milestoneService,
            VillageService villageService
        ) : base(dbService, utilsService, milestoneService, villageService) { }

        public PathDemo GenerateDemo()
        {
            SetNow(8, 1, 3);
            var user = LoadUser("463377", 3);
            
            GenerateDemoPath(
                "463377",
                2,
                "Бег 1км",
                "🏃🏻‍♂️",
                2,
                new[] {1, 2, 3, 4, 5},
                new[]
                {
                    (17, 8),
                    (18, 8),
                    (19, 8),
                    (20, 8),
                    (24, 8),
                    (25, 8),
                    (26, 8),
                    (27, 8),
                    (28, 8),
                    (31, 8),
                    (1, 9),
                    (2, 9),
                    (3, 9),
                    (4, 9),
                    (7, 9),
                    (8, 9),
                    (9, 9),
                    (10, 9),
                    (11, 9),
                }
            );
            
            GenerateDemoPath(
                "463377",
                1,
                "Пить воду",
                "💧",
                4,
                new[] {1, 2, 3, 4, 5, 6, 7},
                new[]
                {
                    (26, 8),
                    (27, 8),
                    (28, 8),
                    (31, 8),
                    (1, 9),
                    (2, 9),
                    (3, 9),
                    (4, 9),
                    (5, 9),
                    (6, 9),
                    (7, 9),
                    (8, 9),
                    (9, 9),
                    (10, 9),
                    (11, 9),
                }
            );
            
            GenerateDemoPath(
                "463377",
                3,
                "Игра на гитаре",
                "🎸",
                1,
                new[] {1, 3, 5},
                new[]
                {
                    (26, 8),
                    (28, 8),
                    (31, 8),
                    (2, 9),
                    (4, 9),
                    (7, 9),
                    (9, 9),
                    (11, 9),
                }
            );

            return new PathDemo(user.User);
        }

        private void GenerateDemoPath(
            string userId, int pathId, string name, string icon, int color, int[] days, (int, int)[] doneDates
        )
        {
            CreateEditPath(userId, pathId, name, icon, color, days, -1, 3);
            foreach (var (d, m) in doneDates)
            {
                SetNow(m, d, 3);
                SetDone(userId, pathId, 3);
            }
        }

        public void SetNow(int month, int day, int offset, int year = 2020)
        {
            _now = new DateTimeOffset(new DateTime(year, month, day, 0, 0, 0), TimeSpan.FromHours(offset));
        }

        protected override DateTimeOffset Now(int offset)
        {
            return _now;
        }
    }
}