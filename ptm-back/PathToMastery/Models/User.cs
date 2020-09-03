using Newtonsoft.Json;
using PathToMastery.Models.State;

namespace PathToMastery.Models
{
    public class User : IIdentity
    {
        public string Id { get; set; }
        
        [JsonIgnore]
        public int Seed { get; set; }
        
        [JsonIgnore]
        public long NotifyTime { get; set; }
        [JsonIgnore]
        public int NotifyPathId { get; set; }
        
        [JsonIgnore]
        public PathData First { get; set; }
        [JsonIgnore]
        public PathData Second { get; set; }
        [JsonIgnore]
        public PathData Third { get; set; }
    }
}