using System;
using System.Collections.Generic;
using System.Linq;
using PathToMastery.Models;
using PathToMastery.Models.State;

namespace PathToMastery.Services
{
    public class MilestoneService
    {
        private readonly List<Milestone> _milestones = new List<Milestone>
        {
            new Milestone
            {
                Id = 1,
                DaysNeed = new[] {7},
                DaysDone = 3,
            },
            new Milestone
            {
                Id = 2,
                DaysNeed = new[] {7, 6, 5},
                DaysDone = 7,
            },
            new Milestone
            {
                Id = 3,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 14,
            },
            new Milestone
            {
                Id = 4,
                DaysNeed = new[] {6, 5, 4, 3, 2, 1},
                DaysDone = 21,
            },
            new Milestone
            {
                Id = 5,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 30,
            },
            new Milestone
            {
                Id = 6,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 45,
            },
            new Milestone
            {
                Id = 7,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 60,
            },
            new Milestone
            {
                Id = 8,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 90,
            },
            new Milestone
            {
                Id = 9,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 120,
            },
            new Milestone
            {
                Id = 10,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 150,
            },
            new Milestone
            {
                Id = 11,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 180,
            },
            new Milestone
            {
                Id = 12,
                DaysNeed = new[] {4, 3, 2, 1},
                DaysDone = 210,
            },
            new Milestone
            {
                Id = 13,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 240,
            },
            new Milestone
            {
                Id = 14,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 300,
            },
            new Milestone
            {
                Id = 15,
                DaysNeed = new[] {7, 6, 5, 4, 3, 2, 1},
                DaysDone = 365,
            }
        };
        
        public int FindMilestoneDays(PathData data, DateTimeOffset date, DateTimeOffset earliestLink)
        {
            var daysFromStart = (int)Math.Round((date - earliestLink).TotalDays) + 1;
            var largestMilestone = data.Done.Count > 0 ? data.Done.Max(d => d.MsD) : 0;
            var milestone = _milestones
                .SkipWhile(m => m.DaysDone <= largestMilestone)
                .FirstOrDefault(m => m.DaysNeed.Contains(data.Days.Length) && m.DaysDone <= daysFromStart);
            
            return milestone == null ? 0 : daysFromStart;
        }
    }
}