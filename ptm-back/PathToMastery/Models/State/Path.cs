namespace PathToMastery.Models.State
{
    public class Path
    {
        public bool CanBeDone { get; set; }
        public PathData Data { get; set; }
        public Day[] Days { get; set; }
    }
}