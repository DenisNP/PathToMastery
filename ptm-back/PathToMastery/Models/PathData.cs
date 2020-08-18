using System.Collections.Generic;

namespace PathToMastery.Models
{
    public class PathData
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Color { get; set; } = 1;
        public int[] Days { get; set; } = new int[0];
        public List<DayMeta> Done { get; set; } = new List<DayMeta>();
    }
}