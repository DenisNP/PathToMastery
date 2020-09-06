using System.Linq;

namespace PathToMastery.Models.State
{
    public class PathDataDemo
    {
        public string Name { get; }
        public string Icon { get; }
        public int Color { get; }
        public int[] Days { get; }
        public int Notify { get; }
        public string[] Done { get; }

        public PathDataDemo(PathData data)
        {
            Name = data.Name;
            Icon = data.Icon;
            Color = data.Color;
            Days = data.Days;
            Notify = data.Notify;
            Done = data.Done.Select(d => d.ToString()).ToArray();
        }
    }

    public class PathDemo
    {
        public PathDataDemo First { get; }
        public PathDataDemo Second { get; }
        public PathDataDemo Third { get; }

        public PathDemo(User user)
        {
            First = new PathDataDemo(user.First);
            Second = new PathDataDemo(user.Second);
            Third = new PathDataDemo(user.Third);
        }
    }
}