using System.Collections.Generic;

namespace PathToMastery.Models.State
{
    public class PathData
    {
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public int Color { get; set; } = 0;
        public int[] Days { get; set; } = new int[0];
        public int Notify { get; set; }
        public List<DayMeta> Done { get; set; } = new List<DayMeta>();

        public void Clear()
        {
            Name = "";
            Icon = "";
            Color = 0;
            Days = new int[0];
            Done = new List<DayMeta>();
        }

        public PathMeta ToMeta(int days)
        {
            return new PathMeta
            {
                Name = Name,
                Color = Color,
                Days = days
            };
        }
    }
}