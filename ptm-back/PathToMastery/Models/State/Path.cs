using System.Collections.Generic;

namespace PathToMastery.Models.State
{
    public class Path
    {
        public bool CanBeDone { get; set; }
        public PathData Data { get; set; }
        public List<Day> Days { get; set; } = new List<Day>();
    }
}