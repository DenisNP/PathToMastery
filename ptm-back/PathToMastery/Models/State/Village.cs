using System.Collections.Generic;

namespace PathToMastery.Models.State
{
    public class Village
    {
        public List<Pagoda> Pagodas { get; set; } = new List<Pagoda>();
        public List<PathMeta> Paths { get; set; } = new List<PathMeta>();
    }
}